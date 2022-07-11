using System.ComponentModel.DataAnnotations;

namespace CadastroDeUsinas.Models
{
    public class Usina
    {
        [Key]
        public int UsinaId { get; set; }
        [Required(ErrorMessage = "Informe o UC da Usina")]
        [Display(Name = "UC da Usina")]
        public int UcDaUsina { get; set; }
        [Display(Name = "Ativo")]
        [Required(ErrorMessage = "Informe se é ativo")]
        public bool IsAtivo { get; set; }
        [Display(Name = "Fornecedor")]
        [Required(ErrorMessage = "Informe o nome do fornecedor")]
        public string NomeFornecedor { get; set; }
    }
}
