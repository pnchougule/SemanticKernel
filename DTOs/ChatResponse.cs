namespace SemanticKernalTest.DTOs
{

    public record class ChatResponse
    {
        public string? chatResponse { get; set; }
        public string? serializedPlan { get; set; }
    }
}
