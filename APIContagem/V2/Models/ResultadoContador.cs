namespace APIContagem.V2.Models;

public class ResultadoContador
{
    public int ValorAtual { get; set; }
    public string? Versao { get; set; }
    public string? Local { get; set; }
    public string? Kernel { get; set; }
    public string? Framework { get; set; }
    public string? Mensagem { get; set; }
}