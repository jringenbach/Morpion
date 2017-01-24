using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morpion
{
	class Program
	{
		static void Main(string[] args)
		{
											//*****************************************************
											//         DECLARATION DES VARIABLES GLOBALES
											//*****************************************************

			int choixMenu = 0;
			bool[] typeDePartie = { false, false };     //La première case concerne la partie contre l'ordinateur
														//la seconde case concerne la partie contre un autre joueur
			char[,] tableauDuMorpion = new char[9,9];
			bool tourDeLUtilisateur = true;

			char symboleJoueur1 = ' ';
			char symboleJoueur2 = ' ';
			char symboleOrdinateur = ' ';
			

			//A partir de cette fonction, on va connaître quel mode de jeu auquel le joueur veut jouer que ce soit grâce aux arguments
			//dans l'invite de commande ou au choix d'une option dans le menu principal du jeu
			MethodePermettantDeLancerLeModeDeJeuDesire(typeDePartie, choixMenu, args);
			Console.Clear();


											//**********************************************
											//         PARTIE CONTRE L'ORDINATEUR
											//**********************************************

			if(typeDePartie[0] == true && typeDePartie[1] == false)
			{
				//Si c'est au tour de l'utilisateur, il choisit son symbole et l'ordinateur aura l'autre symbole
				if (tourDeLUtilisateur == true)
				{
					symboleJoueur1 = choixSymbole(tourDeLUtilisateur);
					symboleOrdinateur = assignationDUnSymboleALautreJoueur(symboleJoueur1);
					tourDeLUtilisateur = false;
				}

				//Si c'est au tour de l'ordinateur de jouer
				else
				{
					symboleOrdinateur = choixSymbole(tourDeLUtilisateur);
					symboleJoueur1 = assignationDUnSymboleALautreJoueur(symboleOrdinateur);
					tourDeLUtilisateur = true;
				}

			}

											//**********************************************
											//              JOUEUR VERSUS JOUEUR
											//**********************************************

			else if (typeDePartie[0] == false && typeDePartie[1] == true)
			{

			}

		}


		//*************************************************************
		//    FONCTIONS CONCERNANT TOUS LES CHOIX DU MENU PRINCIPAL
		//*************************************************************

		static void choixDeLaPartie(bool[] typeDePartie, bool partieOrdinateur, bool partie2Joueurs)
		{
			typeDePartie[0] = partieOrdinateur;
			typeDePartie[1] = partie2Joueurs;
		}

		static void MethodePermettantDeLancerLeModeDeJeuDesire(bool[] typeDePartie, int choixMenu, string[] args)
		{
			//Si on envoie pas d'argument dans l'invite de commande, on lance le menu principal
			if (args.Length == 0)
			{
				choixMenu = MenuPrincipal();

				switch (choixMenu)
				{
					//Si le joueur choisit le premier Menu, il jouera contre l'ordinateur
					case 1:
						choixDeLaPartie(typeDePartie, true, false);
						break;

					//Si le joueur choisit le deuxième Menu, il jouera contre un autre joueur (sur le même ordinateur)
					case 2:
						choixDeLaPartie(typeDePartie, false, true);
						break;

					default:
						Environment.Exit(0);
						break;
				}

			}

			//Si l'utilisateur a entré au moins un argument, on les teste pour savoir quelle fonction du programme il désire lancer
			else
			{
				switch (args[0])
				{
					//Si l'argument est "computer", le joueur jouera contre l'ordinateur
					case "computer":
						choixDeLaPartie(typeDePartie, true, false);
						break;

					//Si l'argument est "2players", l'utilisateur lancera automatiquement le mode 2 joueurs
					case "2players":
						choixDeLaPartie(typeDePartie, false, true);
						break;

					default:
						break;
				}

			}
		}

		static int MenuPrincipal()
		{
			int choixMenu;
			Console.WriteLine("\t\t --- Morpion ---");
			Console.WriteLine("\t\t 1. Jouer contre l'ordinateur");
			Console.WriteLine("\t\t 2. Jouer contre un autre joueur");
			Console.WriteLine("\t\t 3. Quitter");

			//Tant que le joueur ne rentre pas un choix de menu valide, on le force à ressaisir un nombre
			do
			{
				choixMenu = ReadInt(); //On lit le choix du joueur
				if (choixMenu < 0 || choixMenu > 3)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Veuillez saisir un nombre correspondant à une des options du menu.");
					Console.ResetColor();
				}

			} while (choixMenu < 1 || choixMenu > 3);


			return choixMenu;
		}

		//*************************************************************
		//              FONCTIONS DE LA PARTIE DE MORPION
		//*************************************************************

		static char choixSymbole(bool tourDeLUtilisateur)
		{
			char symbole = ' ';
			bool symboleValide = false;

			//Si c'est au tour du joueur de choisir son symbole
			if(tourDeLUtilisateur == true)
			{
				do
				{
					Console.Clear();

					//Affichage du titre du menu de choix des symboles en rouge
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("\t\t\t --- 'O' ou 'X' ---");
					Console.ResetColor();

					//Affichage des explications en jaune
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine("\n\t\t Ecrivez x ou o pour choisir votre camp");
					Console.WriteLine("\t\t Votre choix : ");
					Console.ResetColor();

					symbole = Convert.ToChar(Console.ReadLine());
					symboleValide = (symbole == 'x' || symbole == 'o' || symbole == 'X' || symbole == 'O') ? true : false;

				} while (symboleValide == false);

				symbole = Char.ToUpper(symbole); //Une fois le caractère choisi, on le passe en majuscule
			}

			else
			{
				Random nbAleatoire = new Random();
				int nombreGenere = 0;
				nombreGenere = nbAleatoire.Next(0, 1);

				if(nombreGenere == 0) symbole = 'O';

				else symbole = 'X';

			}

			return symbole;
		}

		static char assignationDUnSymboleALautreJoueur(char symboleJ1)
		{
			char symboleJ2 = ' ';

			if (symboleJ1 == 'X') symboleJ2 = 'O';
			else symboleJ2 = 'X';

			return symboleJ2;
		}

		static void affichageTableauMorption()
		{

		}


		//*************************************
		//      FONCTIONS UTILITAIRES
		//*************************************


		static int ReadInt()
		{
			int nombreLu = 0;

			int.TryParse(Console.ReadLine(), out nombreLu);

			return nombreLu;
		}

	}
}
