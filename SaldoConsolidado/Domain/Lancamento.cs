using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaldoConsolidado.Domain;

public class Lancamento
{
    public Guid Id { get; set; }
    public string Tipo { get; set; } // "debito" ou "credito"
    public decimal Valor { get; set; }
    public string Descricao { get; set; }
    public DateTime DataLancamento { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataAtualizacao { get; set; }
}