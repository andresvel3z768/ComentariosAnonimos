namespace ComentariosAnonimos.Models;

public static class NombreGenerator
{
    private static readonly string[] Abstractos =
    [
        "Niebla", "Eclipse", "Sombra", "Vacio", "Eco",
        "Bruma", "Velo", "Crepusculo", "Horizonte", "Abismo",
        "Silencio", "Aurora", "Caos", "Eter", "Fulgor",
        "Halito", "Latido", "Vislumbre", "Paramo", "Umbral",
        "Resquicio", "Reflejo", "Neblina", "Penumbra", "Estela"
    ];

    private static readonly string[] Neutros =
    [
        "Del", "De_La", "Sin", "Entre", "Bajo",
        "Sobre", "Allende", "Tras", "Cerca_Del", "Mas_Alla_Del"
    ];

    private static readonly string[] Abstractos2 =
    [
        "Eco", "Olvido", "Vacio", "Letargo", "Sosiego",
        "Vértigo", "Fugacidad", "Inercia", "Naufragio", "Pausa",
        "Vacilo", "Entropia", "Simiente", "Rescoldo", "Ceniza",
        "Polvo", "Reflejo", "Suscitar", "Permanencia", "Instantanea"
    ];

    public static string Generar()
    {
        var rng = Random.Shared;
        var a1 = Abstractos[rng.Next(Abstractos.Length)];
        var n1 = Neutros[rng.Next(Neutros.Length)];
        var a2 = Abstractos2[rng.Next(Abstractos2.Length)];
        var num = rng.Next(10, 100);

        return $"{a1}.{n1}.{a2}.{num}";
    }
}
