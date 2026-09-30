using ExercicesLinq;
using static ExercicesLinq.Affichage;

// =====================================================================
// CORRIGÉ — Exercices LINQ d'entraînement (Module 2, complément Atelier 1)
// Document réservé au formateur : ne pas diffuser avant la fin de la séance.
// Chaque exercice indique en commentaire le résultat de contrôle attendu.
// =====================================================================

var clients = JeuDeDonnees.Clients;
var produits = JeuDeDonnees.Produits;
var commandes = JeuDeDonnees.Commandes;

// Une string implémente IEnumerable<char> : Afficher("titre", "texte") choisirait la
// surcharge générique et afficherait un caractère par ligne. Ce petit utilitaire
// force la surcharge « valeur unique » pour afficher un texte.
static void Texte(string titre, string texte) => Afficher(titre, (object)texte);

// =====================================================================
// SÉRIE 1 — Filtrer, trier, projeter
// =====================================================================

// 1.1 — Produits informatiques, par ordre alphabétique
// Contrôle : 4 noms ; le premier est « Clavier mécanique »
var res11 = produits
    .Where(p => p.Categorie == "Informatique")
    .OrderBy(p => p.Nom)
    .Select(p => p.Nom);
Afficher("1.1 Produits informatiques", res11);

// 1.2 — Produits en rupture de stock, du plus cher au moins cher
// Contrôle : 3 produits ; le premier est « Webcam HD » (59,90)
var res12 = produits
    .Where(p => p.Stock == 0)
    .OrderByDescending(p => p.Prix)
    .Select(p => new { p.Nom, p.Prix });
Afficher("1.2 Ruptures de stock", res12);

// 1.3 — Clients particuliers de Paris ou Marseille, du plus ancien au plus récent
// Contrôle : 3 clients ; premier « Martin Léa », dernier « Roux Camille »
var res13 = clients
    .Where(c => c.Segment == "Particulier" && (c.Ville == "Paris" || c.Ville == "Marseille"))
    .OrderBy(c => c.DateInscription)
    .Select(c => new { c.Nom, c.Ville });
Afficher("1.3 Particuliers Paris / Marseille", res13);

// 1.4 — Livraisons du premier semestre 2025
// Contrôle : 10 commandes ; la dernière est la n°12 (25/06/2025)
var debutS1 = new DateTime(2025, 1, 1);
var finS1 = new DateTime(2025, 6, 30);
var res14 = commandes
    .Where(c => c.Statut == "Livree" && c.DateCommande >= debutS1 && c.DateCommande <= finS1)
    .OrderBy(c => c.DateCommande)
    .Select(c => new { c.Id, c.DateCommande });
Afficher("1.4 Livraisons du 1er semestre", res14);

// 1.5 — Prix TTC (TVA 20 %) des produits dont le prix HT dépasse 100 €
// Contrôle : 3 produits ; le premier est « Bureau réglable » à 538,80
var res15 = produits
    .Where(p => p.Prix > 100)
    .OrderByDescending(p => p.Prix)
    .Select(p => new { p.Nom, PrixTTC = Math.Round(p.Prix * 1.2m, 2, MidpointRounding.AwayFromZero) });

Afficher("1.5 Prix TTC haut de gamme", res15);

// 1.6 — Recherche du mot « écran » sans tenir compte de la casse
// Contrôle : « Écran 27 pouces » et « Support d'écran »
var res16 = produits
    .Where(p => p.Nom.Contains("écran", StringComparison.OrdinalIgnoreCase))
    .Select(p => p.Nom);
Afficher("1.6 Produits contenant « écran »", res16);

// =====================================================================
// SÉRIE 2 — Tester, compter, extraire
// =====================================================================

// 2.1 — Commandes annulées : existence puis nombre
// Contrôle : true ; 2
Afficher("2.1 Existe-t-il une commande annulée ?", commandes.Any(c => c.Statut == "Annulee"));
Afficher("2.1 Nombre de commandes annulées", commandes.Count(c => c.Statut == "Annulee"));

// 2.2 — Quantificateurs sur le catalogue
// Contrôle : true ; false
Afficher("2.2 Tous les prix > 5 € ?", produits.All(p => p.Prix > 5));
Afficher("2.2 Tous les produits en stock ?", produits.All(p => p.Stock > 0));

// 2.3 — Première commande d'un client
// Contrôle : commande n°3 pour le client 3 ; null pour le client 8
var premiere3 = commandes
    .Where(c => c.ClientId == 3)
    .OrderBy(c => c.DateCommande)
    .First();
Afficher("2.3 Première commande du client 3", new { premiere3.Id, premiere3.DateCommande });

var premiere8 = commandes
    .Where(c => c.ClientId == 8)
    .OrderBy(c => c.DateCommande)
    .FirstOrDefault();
Afficher("2.3 Première commande du client 8", premiere8);
// Avec First(), le client 8 (aucune commande) provoquerait une InvalidOperationException
// (« Sequence contains no elements ») : First suppose que l'élément existe.
// FirstOrDefault renvoie la valeur par défaut du type, ici null pour une classe.

// 2.4 — Catégories sans doublon, par ordre alphabétique
// Contrôle : Accessoires, Bureau, Informatique, Mobilier
var res24 = produits
    .Select(p => p.Categorie)
    .Distinct()
    .OrderBy(x => x);
Afficher("2.4 Catégories", res24);

// 2.5 — Villes distinctes des clients professionnels, en une seule chaîne
// Contrôle : « Lille, Lyon, Marseille »
var villesEntreprises = string.Join(", ", clients
    .Where(c => c.Segment == "Entreprise")
    .Select(c => c.Ville)
    .Distinct()
    .OrderBy(v => v));
Texte("2.5 Villes des entreprises", villesEntreprises);

// =====================================================================
// SÉRIE 3 — Agréger
// =====================================================================

// 3.1 — Volume des ventes
// Contrôle : 35 lignes ; 83 articles (hors commandes annulées)
var nbLignes = commandes.SelectMany(c => c.Lignes).Count();
var quantiteVendue = commandes
    .Where(c => c.Statut != "Annulee")
    .SelectMany(c => c.Lignes)
    .Sum(l => l.Quantite);
Texte("3.1 Volume des ventes", $"{nbLignes} lignes ; {quantiteVendue} articles vendus (hors annulées)");

// 3.2 — Statistiques de prix
// Contrôle : moyenne ≈ 116,48 ; min 8,90 ; max 449,00 (« Bureau réglable »)
var prixMoyen = produits.Average(p => p.Prix);
var prixMin = produits.Min(p => p.Prix);
var prixMax = produits.Max(p => p.Prix);
var produitLePlusCher = produits.MaxBy(p => p.Prix);
Texte("3.2 Statistiques de prix",
    $"moyenne {prixMoyen:N2} ; min {prixMin:N2} ; max {prixMax:N2} ; le plus cher : {produitLePlusCher?.Nom}");

// 3.3 — Chiffre d'affaires livré (PrixUnitaire de la ligne, pas le prix catalogue)
// Contrôle : 5 314,50 € sur 11 commandes
var commandesLivrees = commandes.Where(c => c.Statut == "Livree").ToList();
var caLivre = commandesLivrees
    .SelectMany(c => c.Lignes)
    .Sum(l => l.Quantite * l.PrixUnitaire);
Texte("3.3 Chiffre d'affaires livré", $"{caLivre:N2} € sur {commandesLivrees.Count} commandes");

// 3.4 — Montant de chaque commande non annulée, moyenne et maximum
// Contrôle : 18 commandes ; moyenne ≈ 438,86 ; maximum : commande n°3 (1 456,00)
var montantsCommandes = commandes
    .Where(c => c.Statut != "Annulee")
    .Select(c => new { c.Id, Montant = c.Lignes.Sum(l => l.Quantite * l.PrixUnitaire) })
    .ToList();
Afficher("3.4 Montant par commande", montantsCommandes);
var montantMoyen = montantsCommandes.Average(m => m.Montant);
var commandeMax = montantsCommandes.MaxBy(m => m.Montant);
Texte("3.4 Moyenne et maximum",
    $"moyenne {montantMoyen:N2} ; plus élevée : n°{commandeMax?.Id} ({commandeMax?.Montant:N2})");

// 3.5 — Valeur du stock
// Contrôle : 14 772,00 € ; « Écran 27 pouces » (2 632,00)
var valeurStock = produits.Sum(p => p.Prix * p.Stock);
var produitStockMax = produits.MaxBy(p => p.Prix * p.Stock);
Texte("3.5 Valeur du stock",
    $"{valeurStock:N2} € ; plus forte valeur : {produitStockMax?.Nom} ({produitStockMax?.Prix * produitStockMax?.Stock:N2})");

// =====================================================================
// SÉRIE 4 — Regrouper
// =====================================================================

// 4.1 — Produits par catégorie
// Contrôle : Informatique 4 ; Bureau 3 ; Accessoires 3 ; Mobilier 2
var res41 = produits
    .GroupBy(p => p.Categorie)
    .Select(g => new { Categorie = g.Key, Nombre = g.Count() });
Afficher("4.1 Produits par catégorie", res41);

// 4.2 — Commandes par statut
// Contrôle : Livree 11 ; EnCours 4 ; Expediee 3 ; Annulee 2
var res42 = commandes
    .GroupBy(c => c.Statut)
    .Select(g => new { Statut = g.Key, Nombre = g.Count() });
Afficher("4.2 Commandes par statut", res42);

// 4.3 — Stock total par catégorie, de la plus fournie à la moins fournie
// Contrôle : Bureau 360 ; Accessoires 75 ; Informatique 33 ; Mobilier 8
var res43 = produits
    .GroupBy(p => p.Categorie)
    .Select(g => new { Categorie = g.Key, StockTotal = g.Sum(p => p.Stock) })
    .OrderByDescending(x => x.StockTotal);
Afficher("4.3 Stock par catégorie", res43);

// 4.4 — Chiffre d'affaires mensuel (commandes non annulées)
// Contrôle : 9 mois ; janvier 911,00 ; février 1 735,20 ; septembre 1 498,60
var res44 = commandes
    .Where(c => c.Statut != "Annulee")
    .GroupBy(c => new { c.DateCommande.Year, c.DateCommande.Month })
    .OrderBy(g => g.Key.Year)
    .ThenBy(g => g.Key.Month)
    .Select(g => new
    {
        g.Key.Year,
        g.Key.Month,
        CA = g.SelectMany(c => c.Lignes).Sum(l => l.Quantite * l.PrixUnitaire)
    });
Afficher("4.4 Chiffre d'affaires mensuel", res44);

// 4.5 — Produit le plus cher de chaque catégorie
// Contrôle : Informatique → Écran 27 pouces ; Mobilier → Bureau réglable ;
//            Bureau → Lampe LED ; Accessoires → Support d'écran
var res45 = produits
    .GroupBy(p => p.Categorie)
    .Select(g => new { Categorie = g.Key, ProduitPhare = g.MaxBy(p => p.Prix)?.Nom });
Afficher("4.5 Produit phare par catégorie", res45);

// =====================================================================
// SÉRIE 5 — Jointures et aplatissement
// =====================================================================

// 5.1 — Commandes et clients
// Contrôle : 20 lignes ; la commande n°1 appartient à « Dupont SA »
var res51 = commandes.Join(
    clients,
    c => c.ClientId,
    cl => cl.Id,
    (c, cl) => new { c.Id, c.DateCommande, Client = cl.Nom });
Afficher("5.1 Commandes et clients", res51);

// 5.2 — Détail des lignes vendues : SelectMany puis Join avec les produits
// Contrôle : 35 lignes ; la plus élevée : commande n°9, « Écran 27 pouces », 1 047,00
var detailLignes = commandes
    .SelectMany(c => c.Lignes, (c, l) => new { CommandeId = c.Id, l.ProduitId, l.Quantite, l.PrixUnitaire })
    .Join(
        produits,
        x => x.ProduitId,
        p => p.Id,
        (x, p) => new { x.CommandeId, Produit = p.Nom, x.Quantite, Total = x.Quantite * x.PrixUnitaire })
    .ToList();
Afficher("5.2 Détail des lignes", detailLignes);
Afficher("5.2 Ligne la plus élevée", detailLignes.MaxBy(x => x.Total));

// 5.3 — Trois meilleurs clients (commandes non annulées)
// Contrôle : Bernard & Fils 3 340,50 ; Dupont SA 2 588,50 ; Garcia Import 622,50
var res53 = commandes
    .Where(c => c.Statut != "Annulee")
    .Join(
        clients,
        c => c.ClientId,
        cl => cl.Id,
        (c, cl) => new { Client = cl.Nom, Montant = c.Lignes.Sum(l => l.Quantite * l.PrixUnitaire) })
    .GroupBy(x => x.Client)
    .Select(g => new { Client = g.Key, CA = g.Sum(x => x.Montant) })
    .OrderByDescending(x => x.CA)
    .Take(3);
Afficher("5.3 Meilleurs clients", res53);

// 5.4 — Clients sans commande, par deux approches
// Contrôle : « Lambert Sophie » (1 client)
var sansCommandeAny = clients
    .Where(cl => !commandes.Any(c => c.ClientId == cl.Id))
    .Select(cl => cl.Nom);
Afficher("5.4 Clients sans commande (Where + !Any)", sansCommandeAny);

var sansCommandeGroupJoin = clients
    .GroupJoin(
        commandes,
        cl => cl.Id,
        c => c.ClientId,
        (cl, cmds) => new { cl.Nom, NbCommandes = cmds.Count() })
    .Where(x => x.NbCommandes == 0)
    .Select(x => x.Nom);
Afficher("5.4 Clients sans commande (GroupJoin)", sansCommandeGroupJoin);

// 5.5 — Produits jamais vendus (aucune commande non annulée)
// Contrôle : « Webcam HD » (1 produit)
var idsVendus = commandes
    .Where(c => c.Statut != "Annulee")
    .SelectMany(c => c.Lignes)
    .Select(l => l.ProduitId)
    .Distinct()
    .ToList();
var res55 = produits
    .Where(p => !idsVendus.Contains(p.Id))
    .Select(p => p.Nom);
Afficher("5.5 Produits jamais vendus", res55);

// 5.6 — Lignes dont le prix payé diffère du prix catalogue actuel
// Contrôle : 3 lignes — commandes n°3, n°9 et n°11
var res56 = commandes
    .SelectMany(c => c.Lignes, (c, l) => new { CommandeId = c.Id, Ligne = l })
    .Join(
        produits,
        x => x.Ligne.ProduitId,
        p => p.Id,
        (x, p) => new { x.CommandeId, Produit = p.Nom, Paye = x.Ligne.PrixUnitaire, Actuel = p.Prix })
    .Where(x => x.Paye != x.Actuel);
Afficher("5.6 Écarts de prix", res56);

// =====================================================================
// SÉRIE 6 — Pagination, syntaxe de requête, exécution différée
// =====================================================================

// 6.1 — Page 2 du catalogue trié par prix décroissant, pages de 4 produits
// Contrôle : Webcam HD 59,90 ; Souris ergonomique 45,00 ; Support d'écran 39,90 ; Lampe LED 34,50
var numeroPage = 2;
var taillePage = 4;
var res61 = produits
    .OrderByDescending(p => p.Prix)
    .Skip((numeroPage - 1) * taillePage)
    .Take(taillePage)
    .Select(p => new { p.Nom, p.Prix });
Afficher("6.1 Page 2 du catalogue", res61);

// 6.2 — Les 3 clients inscrits depuis le plus longtemps
// Contrôle : Garcia Import, Bernard & Fils, Dupont SA
var res62 = clients
    .OrderBy(c => c.DateInscription)
    .Take(3)
    .Select(c => c.Nom);
Afficher("6.2 Clients historiques", res62);

// 6.3 — Catégories dont tous les produits sont en stock
// Contrôle : Mobilier et Bureau
var res63 = produits
    .GroupBy(p => p.Categorie)
    .Where(g => g.All(p => p.Stock > 0))
    .Select(g => g.Key);
Afficher("6.3 Catégories sans rupture", res63);

// 6.4 — Exercices 4.1 et 4.2 réécrits en syntaxe de requête
// Contrôle : résultats identiques à ceux de 4.1 et 4.2
var res64a =
    from p in produits
    group p by p.Categorie into g
    select new { Categorie = g.Key, Nombre = g.Count() };
Afficher("6.4 Produits par catégorie (syntaxe de requête)", res64a);

var res64b =
    from c in commandes
    group c by c.Statut into g
    select new { Statut = g.Key, Nombre = g.Count() };
Afficher("6.4 Commandes par statut (syntaxe de requête)", res64b);

// 6.5 — Exécution différée
// Contrôle : 9 en stock au départ ; 10 avec la requête différée après l'ajout ;
//            9 si la requête a été matérialisée avant l'ajout
var copieProduits = new List<Produit>(JeuDeDonnees.Produits);

var enStockDiffere = copieProduits.Where(p => p.Stock > 0);              // requête : rien n'est encore exécuté
var enStockMaterialise = copieProduits.Where(p => p.Stock > 0).ToList(); // ToList() exécute immédiatement

Afficher("6.5 Avant l'ajout", enStockDiffere.Count());

copieProduits.Add(new Produit { Id = 99, Nom = "Produit de test", Categorie = "Bureau", Prix = 10m, Stock = 5 });

Afficher("6.5 Requête différée, après l'ajout", enStockDiffere.Count());          // 10 : la requête est ré-exécutée sur la source modifiée
Afficher("6.5 Requête matérialisée, après l'ajout", enStockMaterialise.Count);     // 9 : instantané pris avant l'ajout

// =====================================================================
// DÉFI FINAL — Tableau de bord clients
// =====================================================================

// Contrôle : 7 clients (Lambert Sophie absente).
//  Dupont SA     → 4 commandes, 2 588,50 €, dernière le 14/09/2025, catégorie Informatique
//  Garcia Import → 2 commandes,   622,50 €, dernière le 02/09/2025, catégorie Bureau
var produitsParId = produits.ToDictionary(p => p.Id);

var tableauDeBord = commandes
    .Where(c => c.Statut != "Annulee")
    .GroupBy(c => c.ClientId)
    .Join(
        clients,
        g => g.Key,
        cl => cl.Id,
        (g, cl) => new
        {
            Client = cl.Nom,
            NbCommandes = g.Count(),
            CA = g.SelectMany(c => c.Lignes).Sum(l => l.Quantite * l.PrixUnitaire),
            DerniereCommande = g.Max(c => c.DateCommande).ToString("dd/MM/yyyy"),
            CategoriePreferee = g
                .SelectMany(c => c.Lignes)
                .GroupBy(l => produitsParId[l.ProduitId].Categorie)
                .OrderByDescending(gc => gc.Sum(l => l.Quantite))
                .First().Key
        })
    .OrderByDescending(x => x.CA);
Afficher("Défi — Tableau de bord clients", tableauDeBord);
