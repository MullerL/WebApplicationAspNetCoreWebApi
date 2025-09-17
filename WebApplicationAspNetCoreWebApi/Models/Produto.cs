using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplicationAspNetCoreWebApi.Models;

public partial class Produto
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Preco { get; set; }

    public int Quantidade { get; set; }

    public DateTime Criado { get; set; } = DateTime.Now;
    public DateTime Alterado { get; set; } = DateTime.Now;
}
