namespace PRUEBA.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string Correo { get; set; } = null!;

        public string Telefono { get; set; }

        public string Password { get; set; } = null!;
    }
}