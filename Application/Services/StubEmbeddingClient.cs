using Gerald.Application.Interfaces;

namespace Gerald.Application.Services;

public sealed class StubEmbeddingClient : IEmbeddingClient
{
    public IReadOnlyList<float> CreateEmbedding(string text)
    {
        // Stable placeholder vector; replace this adapter with a provider embedding call in production.
        var hash = StringComparer.Ordinal.GetHashCode(text);
        return new[] { (hash & 255) / 255f, ((hash >> 8) & 255) / 255f, ((hash >> 16) & 255) / 255f };
    }
}
