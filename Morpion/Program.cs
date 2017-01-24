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

			bool caseValide = false;
			bool joueur1AGagne = false;
			bool ordinateurAGagne = false;
			bool partieTerminee = false;
			bool rejouer = true;
			bool tourDeLUtilisateur = true;

			bool[] typeDePartie = { false, false };     //La première case concerne la partie contre l'ordinateur
														//la seconde case concerne la partie contre un autre joueur

			char deplacementChoisi = ' ';
			char symboleJoueur1 = ' ';
			char symboleJoueur2 = ' ';
			char symboleOrdinateur = ' ';

			char[,] tableauDuMorpion = new char[3, 3];

			int choixMenu = 0;
			int tourDeJeu = 1;
			int victoireJoueur = 0;
			int victoireOrdinateur = 0;
			int[] positionDuCurseur = new int[2];


			//A partir de cette fonction, on va connaître quel mode de jeu auquel le joueur veut jouer que ce soit grâce aux arguments
			//dans l'invite de commande ou au choix d'une option dans le menu principal du jeu

			do
			{
				MethodePermettantDeLancerLeModeDeJeuDesire(typeDePartie, choixMenu, args);
				Console.Clear();
				//**********************************************
				//         PARTIE CONTRE L'ORDINATEUR
				//**********************************************

				if (typeDePartie[0] == true && typeDePartie[1] == false)
				{
					//Si c'est au tour de l'utilisateur, il choisit son symbole et l'ordinateur aura l'autre symbole
					if (tourDeLUtilisateur == true)
					{
						symboleJoueur1 = ChoixSymbole(tourDeLUtilisateur);
						symboleOrdinateur = AssignationDUnSymboleALautreJoueur(symboleJoueur1);
					}

					//Si c'est au tour de l'ordinateur de jouer
					else
					{
						symboleOrdinateur = ChoixSymbole(tourDeLUtilisateur);
						symboleJoueur1 = AssignationDUnSymboleALautreJoueur(symboleOrdinateur);
					}

					//Boucle des différentes parties jouées
					do
					{
						InitialisationTableauMorpion(tableauDuMorpion);

						//Boucle des tours de jeu. A chaque nouveau tour de boucle, c'est qu'on a changé de joueur
						do
						{

							InitialisationDuCurseur(positionDuCurseur);

							// Quand c'est au tour du joueur
							if (tourDeLUtilisateur == true)
							{
								// Boucle dans laquelle le joueur est forcé de choisir une case valide
								do
								{
									Console.Clear();
									AffichageTableauMorpion(tableauDuMorpion, positionDuCurseur);
									AfficherLeNomDuJoueurQuiDoitJouer(tourDeLUtilisateur);

									// Si c'est à l'utilisateur du programme de jouer
									deplacementChoisi = DeplacementDuCurseur(positionDuCurseur);

									// Si le joueur valide son déplacement, on regarde si la case qu'il joue est valide
									if (deplacementChoisi == '5')
									{
										caseValide = TestDeLaValiditeDuDeplacement(tableauDuMorpion, positionDuCurseur, caseValide);
									}

								} while (deplacementChoisi != '5' || caseValide == false);

							} // Fin du tour du joueur

							// Quand c'est au tour de l'ordinateur
							else
							{
								Console.WriteLine("Tour de l'ordinateur");
								if (tourDeJeu == 1) ActionDeLordinateurAuPremierTour(positionDuCurseur);
								else if (tourDeJeu == 2) ActionDeLordinateurAuDeuxiemeTour(tableauDuMorpion, positionDuCurseur, symboleOrdinateur);
								else ActionDeLordinateur(tableauDuMorpion, positionDuCurseur, symboleOrdinateur);
							}

							// Modifications de fin de tour
							InsertionDuSymboleDansLeTableauDuMorpion(tableauDuMorpion, positionDuCurseur, tourDeLUtilisateur, symboleJoueur1);
							tourDeLUtilisateur = ChangementDUtilisateur(tourDeLUtilisateur);

							if (tourDeJeu == 5)
							{
								joueur1AGagne = TestVictoire(tableauDuMorpion, symboleJoueur1);
								ordinateurAGagne = TestVictoire(tableauDuMorpion, symboleOrdinateur);
								partieTerminee = (joueur1AGagne == true || ordinateurAGagne == true) ? true : false;
							}

							tourDeJeu++;

						} while (partieTerminee == false);

						//Modifications de fin de partie
						if (ordinateurAGagne) victoireOrdinateur++;
						if (joueur1AGagne) victoireJoueur++;

						rejouer = DemanderSiLeJoueurVeutRejouer();

					} while (rejouer == true);



				}

				//**********************************************
				//              JOUEUR VERSUS JOUEUR
				//**********************************************

				else if (typeDePartie[0] == false && typeDePartie[1] == true)
				{

				}
			} while (true);

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

		static void AfficherLeNomDuJoueurQuiDoitJouer(bool tourDeLutilisateur)
		{
			if (tourDeLutilisateur == true)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("\n\t\t\tJoueur 1 : à toi de jouer !");
				Console.ResetColor();
			}

			else
			{
				Console.ForegroundColor = ConsoleColor.DarkYellow;
				Console.WriteLine("\n\t\t\tC'est au tour de l'ordinateur");
				Console.ResetColor();
			}

		}

		static void AffichageTableauMorpion(char[,] tableauDuMorpion, int[] positionDuCurseur)
		{
			int colonne, ligne;
			for (ligne = 0; ligne < 3; ligne++)
			{
				Console.WriteLine("\t\t\t\t -----------"); //Ligne de tirets dessinée entre chaque ligne
				Console.Write("\t\t\t\t| ");
				for (colonne = 0; colonne < 3; colonne++)
				{
					//Si on arrive sur la case où est positionné le curseur
					if (positionDuCurseur[0] == ligne && positionDuCurseur[1] == colonne)
					{
						//Si la case est vide, on affiche le curseur par une étoile rouge
						Console.ForegroundColor = ConsoleColor.Red;
						if (tableauDuMorpion[ligne, colonne] == ' ')
						{
							Console.Write('*');
						}

						//Sinon on affiche le symbole de la case en rouge pour indiquer que le curseur est dessus
						else
						{
							Console.Write(tableauDuMorpion[ligne, colonne]);
						}
					}

					else
					{
						Console.ForegroundColor = ConsoleColor.DarkYellow;
						Console.Write(tableauDuMorpion[ligne, colonne]); //Affichage du symbole
					}

					Console.ResetColor();
					Console.Write(" | "); //Affichage du séparateur entre deux symboles sur une ligne
				}
				Console.WriteLine();
			}
			Console.WriteLine("\t\t\t\t -----------"); //Ligne dessinée à la fin du tableau
		}

		static char AssignationDUnSymboleALautreJoueur(char symboleJ1)
		{
			char symboleJ2 = ' ';

			if (symboleJ1 == 'X') symboleJ2 = 'O';
			else symboleJ2 = 'X';

			return symboleJ2;
		}

		static bool ChangementDUtilisateur(bool tourDeLutilisateur)
		{
			if (tourDeLutilisateur == true) tourDeLutilisateur = false;
			else tourDeLutilisateur = true;

			return tourDeLutilisateur;
		}

		static char ChoixSymbole(bool tourDeLUtilisateur)
		{
			char symbole = ' ';
			bool symboleValide = false;

			//Si c'est au tour du joueur de choisir son symbole
			if (tourDeLUtilisateur == true)
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

				if (nombreGenere == 0) symbole = 'O';

				else symbole = 'X';

			}

			return symbole;
		}

		static char DeplacementDuCurseur(int[] positionDuCurseur)
		{
			char toucheAppuyee = ' ';

			//On affiche le fonctionnement du gameplay pour déplacer le curseur
			Console.WriteLine("\t\t\tAppuyez sur : \n" +
							  "\n\t\t\t2 : pour descendre\n" +
							  "\t\t\t4 : aller à gauche\n" +
							  "\t\t\t6 : aller à droite\n" +
							  "\t\t\t8 : pour monter\n\n" +
							  "\t\t\t5 : pour valider la position\n");

			toucheAppuyee = Convert.ToChar(Console.ReadKey().KeyChar);
			Console.ForegroundColor = ConsoleColor.Red; //On change la couleur du texte de la console en rouge au cas où on devra afficher un message d'erreur

			switch (toucheAppuyee)
			{
				//Si le joueur décide de descendre
				case '2':

					//Si le curseur est déjà sur la dernière case tout en bas on affiche un message d'erreur, sinon on le déplace
					if (positionDuCurseur[0] == 2) Console.WriteLine("Mouvement Impossible !");
					else positionDuCurseur[0] += 1;

					break;

				//Si le joueur décide d'aller à gauche
				case '4':

					//Si le curseur est déjà sur la case tout à gauche on affiche un message d'erreur, sinon on le déplace
					if (positionDuCurseur[1] == 0) Console.WriteLine("Mouvement Impossible !");
					else positionDuCurseur[1] -= 1;

					break;

				//Si le joueur valide son mouvement
				case '5':
					break;

				//Si le joueur décide d'aller à droite
				case '6':

					//Si le curseur est déjà sur la case tout à droite on affiche un message d'erreur, sinon on le déplace
					if (positionDuCurseur[1] == 2) Console.WriteLine("Mouvement Impossible !");
					else positionDuCurseur[1] += 1;

					break;

				//Si le joueur décide d'aller en haut
				case '8':
					//Si le curseur est déjà tout en haut on affiche un message d'erreur, sinon on le déplace
					if (positionDuCurseur[0] == 0) Console.WriteLine("Mouvement Impossible !");
					else positionDuCurseur[0] -= 1;
					break;


				default:
					break;
			}

			Console.ResetColor();

			return toucheAppuyee;
		}

		static void InitialisationDuCurseur(int[] positionDuCurseur)
		{
			positionDuCurseur[0] = 0;
			positionDuCurseur[1] = 0;
		}

		static void InitialisationTableauMorpion(char[,] tableauDuMorpion)
		{
			int colonne, ligne;

			//On initialise le tableau du morpion en insérant des espaces dans chaque case
			for (ligne = 0; ligne < 3; ligne++)
			{
				for (colonne = 0; colonne < 3; colonne++)
				{
					tableauDuMorpion[ligne, colonne] = ' ';
				}
			}
		}

		static void InsertionDuSymboleDansLeTableauDuMorpion(char[,] tableauDuMorpion, int[] positionDuCurseur, bool tourDeLUtilisateur, char symboleJ1)
		{
			int ligne = positionDuCurseur[0];
			int colonne = positionDuCurseur[1];

			//Si c'était le tour de l'utilisateur, la case du tableau du morpion se voit assigné de son symbole, sinon on y insère
			//le symbole de l'ordinateur
			if (tourDeLUtilisateur == true) tableauDuMorpion[ligne, colonne] = symboleJ1;
			else tableauDuMorpion[ligne, colonne] = AssignationDUnSymboleALautreJoueur(symboleJ1);
		}

		static bool TestDeLaValiditeDuDeplacement(char[,] tableauDuMorpion, int[] positionDuCurseur, bool caseValide)
		{
			int ligne = positionDuCurseur[0];
			int colonne = positionDuCurseur[1];

			if (tableauDuMorpion[ligne, colonne] != ' ')
			{
				Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Cette case est déjà prise !");
				Console.ResetColor();
				caseValide = false;
			}

			else caseValide = true;

			return caseValide;
		}

		static bool TestVictoire(char[,] tableauDuMorpion, char symbole)
		{
			bool victoire = false;
			int ligne = 0, colonne = 0;

			//Analyse des lignes
			for (ligne = 0; ligne < 3; ligne++)
			{
				if (tableauDuMorpion[ligne, 0] == symbole && tableauDuMorpion[ligne, 1] == symbole && tableauDuMorpion[ligne, 2] == symbole)
				{
					victoire = true;
				}
			}

			//Analyse des colonnes - On ne les teste pas si le joueur a déjà gagné sur les lignes
			if (victoire != true)
			{
				for (colonne = 0; colonne < 3; colonne++)
				{
					if (tableauDuMorpion[0, colonne] == symbole && tableauDuMorpion[1, colonne] == symbole && tableauDuMorpion[2, colonne] == symbole)
					{
						victoire = true;
					}
				}
			}

			//Analyse des diagonales
			if (victoire != true)
			{
				if (tableauDuMorpion[0, 0] == symbole && tableauDuMorpion[1, 1] == symbole && tableauDuMorpion[2, 2] == symbole) victoire = true;
				if (tableauDuMorpion[0, 2] == symbole && tableauDuMorpion[1, 1] == symbole && tableauDuMorpion[2, 0] == symbole) victoire = true;
			}

			return victoire;
		}

		static bool DemanderSiLeJoueurVeutRejouer()
		{
			bool rejouer = false;
			char reponseSaisie = ' ';

			do
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Voulez-vous rejouer ?(y/n)");
				Console.ResetColor();

				reponseSaisie = Convert.ToChar(Console.ReadKey().KeyChar);

			} while (reponseSaisie != 'y' && reponseSaisie != 'n');

			if (reponseSaisie == 'y') rejouer = true;
			else rejouer = false;

			return rejouer;
		}

		//*************************************************************
		//                    JEU DE L'ORDINATEUR
		//*************************************************************

		static void ActionDeLordinateurAuPremierTour(int[] positionDuCurseur)
		{
			//L'ordinateur joue au centre du tableau
			positionDuCurseur[0] = 1;
			positionDuCurseur[1] = 1;
		}

		static void ActionDeLordinateurAuDeuxiemeTour(char[,] tableauDuMorpion, int[] positionDuCurseur, char symboleOrdinateur)
		{
			Random aleatoire = new Random();
			int nbAleatoire = aleatoire.Next(0, 3);

			//Si le joueur a joué au centre au premier tour, l'ordinateur jouera dans un coin au hasard
			if (tableauDuMorpion[1, 1] == symboleOppose(symboleOrdinateur))
			{
				switch (nbAleatoire)
				{
					//L'ordinateur joue en haut à gauche
					case 0:
						positionDuCurseur[0] = 0;
						positionDuCurseur[1] = 0;
						break;

					//L'ordinateur joue en haut à droite
					case 1:
						positionDuCurseur[0] = 0;
						positionDuCurseur[1] = 2;
						break;

					//L'ordinateur joue en bas à gauche
					case 2:
						positionDuCurseur[0] = 2;
						positionDuCurseur[1] = 0;
						break;

					//L'ordinateur joue en bas à droite
					case 3:
						positionDuCurseur[0] = 2;
						positionDuCurseur[1] = 2;
						break;

					default:
						break;
				}
			}

			//Si le joueur n'a pas joué au centre, alors l'ordinateur jouera au centre
			else
			{
				positionDuCurseur[0] = 0;
				positionDuCurseur[1] = 0;
			}

		}

		static void ActionDeLordinateur(char[,] tableauDuMorpion, int[] positionDuCurseur, char symboleOrdinateur)
		{
			// L'ORDINATEUR ANALYSE SI IL PEUT GAGNER
			AnalyseDuTableauDuMorpion(tableauDuMorpion, positionDuCurseur, symboleOrdinateur);


			// L'ORDINATEUR ANALYSE SI LE JOUEUR PEUT GAGNER
			AnalyseDuTableauDuMorpion(tableauDuMorpion, positionDuCurseur, symboleOppose(symboleOrdinateur));

			// SINON IL JOUE AU HASARD
			OrdinateurJoueAuHasard(tableauDuMorpion, positionDuCurseur);
		}

		static void AnalyseDuTableauDuMorpion(char[,] tableauDuMorpion, int[] positionDuCurseur, char symbole)
		{
			int ligne = 0, colonne = 0;
			bool coupJoue = false;

			//Analyse des lignes
			for (ligne = 0; ligne < 3; ligne++)
			{
				if (tableauDuMorpion[ligne, 0] == symbole && tableauDuMorpion[ligne, 1] == symbole)
				{
					positionDuCurseur[0] = ligne;
					positionDuCurseur[1] = 2;
					coupJoue = true;
				}

				else if (tableauDuMorpion[ligne, 1] == symbole && tableauDuMorpion[ligne, 2] == symbole)
				{
					positionDuCurseur[0] = ligne;
					positionDuCurseur[1] = 0;
					coupJoue = true;
				}

				else if (tableauDuMorpion[ligne, 0] == symbole && tableauDuMorpion[ligne, 2] == symbole)
				{
					positionDuCurseur[0] = ligne;
					positionDuCurseur[1] = 1;
					coupJoue = true;
				}

			} //Fin de l'analyse des lignes

			//Si l'ordinateur n'a pas trouvé de coup à jouer, il analyse les colonnes
			if (coupJoue == false)
			{
				for (colonne = 0; colonne < 3; colonne++)
				{
					if (tableauDuMorpion[0, colonne] == symbole && tableauDuMorpion[1, colonne] == symbole)
					{
						positionDuCurseur[0] = 2;
						positionDuCurseur[1] = colonne;
						coupJoue = true;
					}

					else if (tableauDuMorpion[1, colonne] == symbole && tableauDuMorpion[2, colonne] == symbole)
					{
						positionDuCurseur[0] = 0;
						positionDuCurseur[1] = colonne;
						coupJoue = true;
					}

					else if (tableauDuMorpion[0, colonne] == symbole && tableauDuMorpion[2, colonne] == symbole)
					{
						positionDuCurseur[0] = 1;
						positionDuCurseur[1] = colonne;
						coupJoue = true;
					}

				}
			}//Fin de l'analyse des colonnes

			//Si il n'a toujours rien trouvé, il analyse ses diagonales
			if (coupJoue == false)
			{

				//Il joue la case haut gauche
				if (tableauDuMorpion[2, 2] == symbole && tableauDuMorpion[1, 1] == symbole)
				{
					positionDuCurseur[0] = 0;
					positionDuCurseur[1] = 0;
					coupJoue = true;
				}

				//Il joue la case haut droite
				else if (tableauDuMorpion[2, 0] == symbole && tableauDuMorpion[1, 1] == symbole)
				{
					positionDuCurseur[0] = 0;
					positionDuCurseur[1] = 2;
					coupJoue = true;
				}

				//Il joue la case bas gauche
				else if (tableauDuMorpion[0, 2] == symbole && tableauDuMorpion[1, 1] == symbole)
				{
					positionDuCurseur[0] = 2;
					positionDuCurseur[1] = 0;
					coupJoue = true;
				}

				//Il joue la case bas droite
				else if (tableauDuMorpion[0, 0] == symbole && tableauDuMorpion[1, 1] == symbole)
				{
					positionDuCurseur[0] = 2;
					positionDuCurseur[1] = 2;
					coupJoue = true;
				}

				//Il joue la case du milieu
				else if (tableauDuMorpion[0, 0] == symbole && tableauDuMorpion[2, 2] == symbole || tableauDuMorpion[0, 2] == symbole && tableauDuMorpion[2, 0] == symbole)
				{
					positionDuCurseur[0] = 1;
					positionDuCurseur[1] = 1;
					coupJoue = true;
				}

			} //Fin de l'analyse des diagonales

		}

		static void OrdinateurJoueAuHasard(char[,] tableauDuMorpion, int[] positionDuCurseur)
		{
			Random hasard = new Random();
			bool caseVide = false;
			int nbAleatoire = hasard.Next(0, 9);

			nbAleatoire = hasard.Next(0, 9);
			do
			{
				switch (nbAleatoire)
				{
					case 0:
						positionDuCurseur[0] = 0;
						positionDuCurseur[1] = 0;
						break;

					case 1:
						positionDuCurseur[0] = 0;
						positionDuCurseur[1] = 1;
						break;

					case 2:
						positionDuCurseur[0] = 0;
						positionDuCurseur[1] = 2;
						break;

					case 3:
						positionDuCurseur[0] = 1;
						positionDuCurseur[1] = 0;
						break;

					case 4:
						positionDuCurseur[0] = 1;
						positionDuCurseur[1] = 1;
						break;

					case 5:
						positionDuCurseur[0] = 1;
						positionDuCurseur[1] = 2;
						break;

					case 6:
						positionDuCurseur[0] = 2;
						positionDuCurseur[1] = 0;
						break;

					case 7:
						positionDuCurseur[0] = 2;
						positionDuCurseur[1] = 1;
						break;

					case 8:
						positionDuCurseur[0] = 2;
						positionDuCurseur[1] = 2;
						break;
				}

				if (tableauDuMorpion[positionDuCurseur[0], positionDuCurseur[1]] == 'X' || tableauDuMorpion[positionDuCurseur[0], positionDuCurseur[1]] == 'O') caseVide = false;
				else caseVide = true;

			} while (caseVide == false);

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

		static char symboleOppose(char symbole)
		{
			if (symbole == 'X') symbole = 'O';
			else symbole = 'X';

			return symbole;
		}

	}
}
