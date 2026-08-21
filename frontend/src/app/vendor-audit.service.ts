import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface AuditFinding {
  id: number;
  vendorId: number;
  severity: string;
  status: string;
  notes: string;
  findingDate: string;
}

export interface Vendor {
  id: number;
  name: string;
  registrationNumber: string;
  registrationDate: string;
  status: string;
  findings: AuditFinding[];
}

export interface RiskAssessment {
  vendorId: number;
  vendorName: string;
  riskRating: string;
  summary: string;
  keyFindings: string[];
  reasoning: string;
}

@Injectable({ providedIn: 'root' })
export class VendorAuditService {
  private readonly http = inject(HttpClient);

  getVendors(): Observable<Vendor[]> {
    return this.http.get<Vendor[]>('/api/vendors');
  }

  getVendor(id: number): Observable<Vendor> {
    return this.http.get<Vendor>(`/api/vendors/${id}`);
  }

  getRiskAssessment(id: number): Observable<RiskAssessment> {
    return this.http.post<RiskAssessment>(`/api/vendors/${id}/risk-assessment`, {});
  }
}
