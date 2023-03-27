public sealed class errorMessage
    {
    public errorMessage(string error)
    {
        this.error = error;
    }

    /// <summary>
    /// Descripción de error
    /// </summary>
    public string error { get; set; }
    }