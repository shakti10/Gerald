import { Component, OnInit, inject } from '@angular/core';
import { DatePipe, DecimalPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuditFinding, RiskAssessment, Vendor, VendorAuditService } from './vendor-audit.service';

@Component({
  selector: 'app-root',
  imports: [DatePipe, DecimalPipe, FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit {
  private readonly auditService = inject(VendorAuditService);

  protected vendors: Vendor[] = [];
  protected selectedVendor: Vendor | null = null;
  protected assessment: RiskAssessment | null = null;
  protected loading = true;
  protected assessing = false;
  protected error = '';
  protected searchTerm = '';
  protected activeFilter = 'All';

  ngOnInit(): void {
    this.loadVendors();
  }

  protected get filteredVendors(): Vendor[] {
    const query = this.searchTerm.trim().toLowerCase();
    return this.vendors.filter((vendor) => {
      const matchesSearch = !query || vendor.name.toLowerCase().includes(query) || vendor.registrationNumber.toLowerCase().includes(query);
      const matchesFilter = this.activeFilter === 'All' || vendor.status === this.activeFilter;
      return matchesSearch && matchesFilter;
    });
  }

  protected get openFindings(): number {
    return this.vendors.reduce((total, vendor) => total + vendor.findings.filter((finding) => finding.status === 'Open').length, 0);
  }

  protected get highRiskCount(): number {
    return this.vendors.filter((vendor) => this.localRisk(vendor) === 'High').length;
  }

  protected selectVendor(vendor: Vendor): void {
    this.selectedVendor = vendor;
    this.assessment = null;
    this.error = '';
    this.auditService.getVendor(vendor.id).subscribe({
      next: (detail) => this.selectedVendor = detail,
      error: () => this.error = 'Unable to load the selected vendor.'
    });
  }

  protected requestAssessment(): void {
    if (!this.selectedVendor) return;
    this.assessing = true;
    this.error = '';
    this.auditService.getRiskAssessment(this.selectedVendor.id).subscribe({
      next: (assessment) => {
        this.assessment = assessment;
        this.assessing = false;
      },
      error: () => {
        this.error = 'Risk assessment could not be generated.';
        this.assessing = false;
      }
    });
  }

  protected severityClass(value: string): string {
    return value.toLowerCase();
  }

  protected localRisk(vendor: Vendor): string {
    const high = vendor.findings.filter((finding) => finding.severity === 'High').length;
    const medium = vendor.findings.filter((finding) => finding.severity === 'Medium').length;
    const open = vendor.findings.filter((finding) => finding.status === 'Open').length;
    if (high >= 2 || (high > 0 && open > 2)) return 'High';
    if (high === 1 || medium >= 3 || open >= 4) return 'Medium';
    return 'Low';
  }

  private loadVendors(): void {
    this.loading = true;
    this.auditService.getVendors().subscribe({
      next: (vendors) => {
        this.vendors = vendors;
        if (vendors.length > 0) this.selectVendor(vendors[0]);
        this.loading = false;
      },
      error: () => {
        this.error = 'The API is unavailable. Start the ASP.NET API on port 5299 and try again.';
        this.loading = false;
      }
    });
  }
}
