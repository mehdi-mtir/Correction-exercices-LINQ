namespace ExercicesLinq;

public class Client
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Ville { get; set; } = string.Empty;
    public string Segment { get; set; } = string.Empty;   // "Particulier" ou "Entreprise"
    public DateTime DateInscription { get; set; }
}

public class Produit
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Categorie { get; set; } = string.Empty;
    public decimal Prix { get; set; }      // prix catalogue HT actuel
    public int Stock { get; set; }
}

public class LigneCommande
{
    public int ProduitId { get; set; }
    public int Quantite { get; set; }
    public decimal PrixUnitaire { get; set; }   // prix figé au moment de la commande
}

public class Commande
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public DateTime DateCommande { get; set; }
    public string Statut { get; set; } = string.Empty;   // EnCours, Expediee, Livree ou Annulee
    public List<LigneCommande> Lignes { get; set; } = new();
}
