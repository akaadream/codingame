using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

class MainGame
{
    static void Main(string[] args)
    {
        Console.Error.WriteLine("Start !");
        // Liste des recettes
        List<Recipe> Recipes = new List<Recipe>();
        // Nombre total de client
        int totalNumberOfCustomers = 0;

        // Variables personnelles
        int bestCustomerAward = 0;
        string bestCustomerItem = "";
        Recipe bestCustomer;

        // Personnages
        Player player = new Player();
        Player partner = new Player();

        // Position des objets de la cuisine
        // Position des assiettes
        int dishesX = 0;
        int dishesY = 0;
        // Position de la glace
        int iceCreamCreateX = 0;
        int iceCreamCreateY = 0;
        // Position des myrtilles
        int blueBerriesCrateX = 0;
        int blueBerriesCrateY = 0;
        // Position de la sonnette pour donner l'assiette au client
        int windowX = 0;
        int windowY = 0;
        // Position des fraises
        int strawberriesX = 0;
        int strawberriesY = 0;

        // Variable pour récupérer les données du premier tour
        string[] inputs;
        // Nombre total de clients
        totalNumberOfCustomers = int.Parse(Console.ReadLine());
        for (int i = 0; i < totalNumberOfCustomers; i++)
        {
            // Attribution des données
            inputs = Console.ReadLine().Split(' ');
            // Ajouter une recette dans la liste des recettes (contenu de la recette, nombre de points donnés)
            Recipes.Add(new Recipe(inputs[0], int.Parse(inputs[1])));
        }

        // Attribution par défaut de la recette
        if (Recipes.Count >= 2) {
            // Au moins 2 recettes
            player.Recipe = Recipes[1];
        }
        else if (Recipes.Count == 1) {
            // Dernière recette
            player.Recipe = Recipes[0];
        }

        // Parcours de la map (y)
        for (int i = 0; i < 7; i++)
        {
            string kitchenLine = Console.ReadLine();
            // Parcours de la map (x)
            for (int j = 0; j < 11; j++)
            {
                // Récupération de la position des assiettes
                if (kitchenLine[j] == 'D') {
                    dishesX = j;
                    dishesY = i;
                }
                // Récupération de la position de la cloche
                else if (kitchenLine[j] == 'W') {
                    windowX = j;
                    windowY = i;
                }
                // Récupération de la position des myrtilles
                else if (kitchenLine[j] == 'B') {
                    blueBerriesCrateX = j;
                    blueBerriesCrateY = i;
                }
                // Récupération de la position de la glace
                else if (kitchenLine[j] == 'I') {
                    iceCreamCreateX = j;
                    iceCreamCreateY = i;
                }
                // Récupération de la position des fraises
                else if (kitchenLine[j] == 'S') {
                    strawberriesX = j;
                    strawberriesY = i;
                }
            }
        }

        // Boucle de jeu
        while (true)
        {
            // Nombre de tours restants
            int turnsRemaining = int.Parse(Console.ReadLine());
            // Récupération des données du joueur
            inputs = Console.ReadLine().Split(' ');
            // Position du joueur
            player.X = int.Parse(inputs[0]);
            player.Y = int.Parse(inputs[1]);
            player.Content = inputs[2];
            // Récupération des données du partenaire
            inputs = Console.ReadLine().Split(' ');
            partner.X = int.Parse(inputs[0]);
            partner.Y = int.Parse(inputs[1]);
            partner.Content = inputs[2];
            // Nombre de tables qui tiennent un objet
            int numTablesWithItems = int.Parse(Console.ReadLine());
            // On parcourt ces tables
            for (int i = 0; i < numTablesWithItems; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int tableX = int.Parse(inputs[0]);
                int tableY = int.Parse(inputs[1]);
                string item = inputs[2];
            }
            // Récupération des données
            inputs = Console.ReadLine().Split(' ');
            string ovenContents = inputs[0]; // ignore until wood 1 league
            int ovenTimer = int.Parse(inputs[1]);
            // Recettes en attente
            int numCustomers = int.Parse(Console.ReadLine()); // the number of customers currently waiting for
            List<Recipe> tempRecipes = new List<Recipe>();
            for (int i = 0; i < numCustomers; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                tempRecipes.Add(new Recipe(inputs[0], int.Parse(inputs[1])));
            }

            // Si le joueur n'a pas encore choisi de recette à faire, on lui attribut une recette
            if (player.Recipe.Given)
            {
                Recipes.Clear();
                Recipes = tempRecipes;

                // Attribution de la recette
                if (Recipes.Count >= 2) {
                    // Au moins 2 recettes
                    player.Recipe = Recipes[1];
                }
                else if (Recipes.Count == 1) {
                    // Dernière recette
                    player.Recipe = Recipes[0];
                }
            }

            // Récupérer le prochain élément de la recette
            Element next = player.Recipe.GetNext();

            // Si le joueur a tous les ingrédients
            if (player.Recipe.HavingEverything()) {
                // Si la commande n'a pas encore été donnée
                if (!player.Recipe.Given) {
                    Console.WriteLine("USE " + windowX + " " + windowY);
                    if (player.X >= windowX - 1 && player.X <= windowX + 1 && player.Y >= windowY - 1 && player.Y <= windowY + 1) {
                        player.Recipe.Given = true;
                    }
                }
                else {
                    // Sinon, on attend la prochaine commande
                    Console.WriteLine("WAIT");
                }
            }
            // Si le joueur à un ingrédient à aller chercher
            else if (next != null) {
                // On vérifie l'ingrédient qu'on doit chercher
                switch (next.Name) {
                    case "DISH":
                        Console.WriteLine("USE " + dishesX + " " + dishesY);
                        // Si le joueur porte une assiette vide
                        if (player.Content.Contains("DISH")) {
                            if (next.Name == "DISH") next.Have = true;
                        }
                        break;
                    case "ICE_CREAM":
                        Console.WriteLine("USE " + iceCreamCreateX + " " + iceCreamCreateY);
                        // Si le joueur à de la glace sur son assiette
                        if (player.Content.Contains("ICE_CREAM")) {
                            if (next.Name == "ICE_CREAM") next.Have = true;
                        }
                        break;
                    case "BLUEBERRIES":
                        Console.WriteLine("USE " + blueBerriesCrateX + " " + blueBerriesCrateY);
                        // Si le joueur à des myrtilles sur son assiette
                        if (player.Content.Contains("BLUEBERRIES")) {
                            if (next.Name == "BLUEBERRIES") next.Have = true;
                        }
                        break;
                    case "CHOPPED_STRAWBERRIES":
                        
                        break;
                    default:
                        // Si aucun cas n'est vérifié, on attend
                        Console.WriteLine("WAIT");
                        break;
                }
            }
            else {
                // On a rien à faire ensuite
                Console.WriteLine("WAIT");
            }
        }
    }
}

class Recipe
{
    public string Content { get; set; }
    public int Award { get; set; }
    public List<Element> Elements { get; set; }
    public bool Given { get; set; }

    // Création de la recette
    public Recipe(string content, int award) {
        Content = content;
        Award = award;
        Given = false;
        Elements =  new List<Element>();
        ParseContent();
    }

    // Récupérer la liste des ingrédients de la recette
    public void ParseContent() {
        string[] temp = Content.Split('-');
        foreach (string element in temp)
        {
            Console.Error.WriteLine("Element added : " + element);
            Elements.Add(new Element(element));
        }
    }

    // Prochain élément à avoir
    public Element GetNext() {
        foreach (Element element in Elements)
        {
            if (!element.Have) return element;
        }

        return null;
    }

    // On vérifie si on a déjà tout complété
    public bool HavingEverything()
    {
        bool have = true;
        foreach (Element element in Elements) if (!element.Have) have = false;
        return have;
    }
}

// Ingrédient de recette
class Element {
    public string Name { get; set; }
    public bool Have { get; set; }

    public Element(string name) {
        // Nom de l'élément
        Name = name;
        // Par défaut, on n'a pas encore récupéré l'ingrédient
        Have = false;
    }
}

class Player
{
    // Position du personnage
    public int X { get; set; }
    public int Y { get; set; }
    // Ce que le personnage porte
    public string Content { get; set; }
    // La recette que le joueur est en train de suivre
    public Recipe Recipe { get; set; }

    public Player()
    {
        X = 0;
        Y = 0;
    }

    // Si le personnage ne porte rien
    public bool GotNothing() {
        return Content == "NONE";
    }
}
