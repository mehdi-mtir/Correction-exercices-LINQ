namespace ExercicesLinq;

public static class JeuDeDonnees
{
    // Date de référence fixe : les résultats ne dépendent jamais de la date du jour
    public static readonly DateTime DateReference = new DateTime(2025, 9, 30);

    public static readonly List<Client> Clients = new()
    {
        new() { Id = 1, Nom = "Dupont SA", Ville = "Lyon", Segment = "Entreprise", DateInscription = new DateTime(2022, 3, 15) },
        new() { Id = 2, Nom = "Martin Léa", Ville = "Paris", Segment = "Particulier", DateInscription = new DateTime(2023, 1, 10) },
        new() { Id = 3, Nom = "Bernard & Fils", Ville = "Lille", Segment = "Entreprise", DateInscription = new DateTime(2021, 11, 2) },
        new() { Id = 4, Nom = "Petit Karim", Ville = "Marseille", Segment = "Particulier", DateInscription = new DateTime(2023, 6, 21) },
        new() { Id = 5, Nom = "Atelier Moreau", Ville = "Lyon", Segment = "Entreprise", DateInscription = new DateTime(2022, 9, 30) },
        new() { Id = 6, Nom = "Roux Camille", Ville = "Paris", Segment = "Particulier", DateInscription = new DateTime(2024, 2, 14) },
        new() { Id = 7, Nom = "Garcia Import", Ville = "Marseille", Segment = "Entreprise", DateInscription = new DateTime(2020, 5, 5) },
        new() { Id = 8, Nom = "Lambert Sophie", Ville = "Bordeaux", Segment = "Particulier", DateInscription = new DateTime(2024, 5, 27) },
    };

    public static readonly List<Produit> Produits = new()
    {
        new() { Id = 1, Nom = "Clavier mécanique", Categorie = "Informatique", Prix = 89.90m, Stock = 25 },
        new() { Id = 2, Nom = "Souris ergonomique", Categorie = "Informatique", Prix = 45.00m, Stock = 0 },
        new() { Id = 3, Nom = "Écran 27 pouces", Categorie = "Informatique", Prix = 329.00m, Stock = 8 },
        new() { Id = 4, Nom = "Webcam HD", Categorie = "Informatique", Prix = 59.90m, Stock = 0 },
        new() { Id = 5, Nom = "Bureau réglable", Categorie = "Mobilier", Prix = 449.00m, Stock = 3 },
        new() { Id = 6, Nom = "Fauteuil ergonomique", Categorie = "Mobilier", Prix = 289.00m, Stock = 5 },
        new() { Id = 7, Nom = "Lampe LED", Categorie = "Bureau", Prix = 34.50m, Stock = 40 },
        new() { Id = 8, Nom = "Cahier A4 (lot de 10)", Categorie = "Bureau", Prix = 12.90m, Stock = 120 },
        new() { Id = 9, Nom = "Stylos (boîte de 50)", Categorie = "Bureau", Prix = 8.90m, Stock = 200 },
        new() { Id = 10, Nom = "Câble USB-C", Categorie = "Accessoires", Prix = 9.90m, Stock = 0 },
        new() { Id = 11, Nom = "Support d'écran", Categorie = "Accessoires", Prix = 39.90m, Stock = 15 },
        new() { Id = 12, Nom = "Hub USB", Categorie = "Accessoires", Prix = 29.90m, Stock = 60 },
    };

    public static readonly List<Commande> Commandes = new()
    {
        C(1, 1, new DateTime(2025, 1, 12), "Livree", L(3, 2, 329.00m), L(1, 2, 89.90m)),
        C(2, 2, new DateTime(2025, 1, 20), "Livree", L(7, 1, 34.50m), L(8, 3, 12.90m)),
        C(3, 3, new DateTime(2025, 2, 3), "Livree", L(5, 2, 439.00m), L(6, 2, 289.00m)),
        C(4, 1, new DateTime(2025, 2, 17), "Livree", L(11, 4, 39.90m), L(12, 4, 29.90m)),
        C(5, 5, new DateTime(2025, 3, 5), "Livree", L(6, 1, 289.00m)),
        C(6, 7, new DateTime(2025, 3, 18), "Annulee", L(3, 1, 329.00m)),
        C(7, 2, new DateTime(2025, 4, 2), "Livree", L(1, 1, 89.90m), L(2, 1, 45.00m), L(10, 2, 9.90m)),
        C(8, 4, new DateTime(2025, 4, 15), "Livree", L(9, 2, 8.90m)),
        C(9, 3, new DateTime(2025, 5, 9), "Livree", L(3, 3, 349.00m), L(11, 3, 39.90m)),
        C(10, 6, new DateTime(2025, 5, 22), "Expediee", L(7, 2, 34.50m), L(12, 1, 29.90m)),
        C(11, 1, new DateTime(2025, 6, 11), "Livree", L(5, 1, 439.00m), L(6, 1, 289.00m), L(7, 2, 34.50m)),
        C(12, 7, new DateTime(2025, 6, 25), "Livree", L(8, 10, 12.90m), L(9, 5, 8.90m)),
        C(13, 5, new DateTime(2025, 7, 8), "Expediee", L(1, 3, 89.90m)),
        C(14, 2, new DateTime(2025, 7, 19), "Annulee", L(4, 1, 59.90m)),
        C(15, 3, new DateTime(2025, 8, 4), "EnCours", L(3, 2, 329.00m), L(12, 2, 29.90m)),
        C(16, 6, new DateTime(2025, 8, 21), "Livree", L(11, 1, 39.90m), L(10, 3, 9.90m)),
        C(17, 7, new DateTime(2025, 9, 2), "EnCours", L(5, 1, 449.00m)),
        C(18, 1, new DateTime(2025, 9, 14), "EnCours", L(1, 5, 89.90m), L(2, 5, 45.00m)),
        C(19, 4, new DateTime(2025, 9, 23), "Expediee", L(6, 1, 289.00m), L(7, 1, 34.50m)),
        C(20, 5, new DateTime(2025, 9, 26), "EnCours", L(8, 4, 12.90m)),
    };

    private static LigneCommande L(int produitId, int quantite, decimal prixUnitaire) =>
        new() { ProduitId = produitId, Quantite = quantite, PrixUnitaire = prixUnitaire };

    private static Commande C(int id, int clientId, DateTime date, string statut, params LigneCommande[] lignes) =>
        new() { Id = id, ClientId = clientId, DateCommande = date, Statut = statut, Lignes = lignes.ToList() };
}
