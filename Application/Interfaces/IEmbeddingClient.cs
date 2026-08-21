namespace Gerald.Application.Interfaces;

public interface IEmbeddingClient
{
    IReadOnlyList<float> CreateEmbedding(string text);
}
