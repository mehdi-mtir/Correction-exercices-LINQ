namespace ExercicesLinq;

public static class Affichage
{
    // Affiche un titre puis chaque élément de la séquence sur sa propre ligne
    public static void Afficher<T>(string titre, IEnumerable<T> elements)
    {
        Console.WriteLine($"--- {titre} ---");
        foreach (var e in elements)
            Console.WriteLine($"  {e}");
        Console.WriteLine();
    }

    // Affiche un titre puis une valeur unique (nombre, booléen, texte...)
    public static void Afficher(string titre, object? valeur)
    {
        Console.WriteLine($"--- {titre} ---");
        Console.WriteLine($"  {valeur ?? "(null)"}");
        Console.WriteLine();
    }
}
