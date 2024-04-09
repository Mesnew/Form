    using System.Collections.ObjectModel;
    using System.IO;
    using System.Windows;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;

namespace Formulaire
    {
        public partial class MainWindow : Window
        {
            public ObservableCollection<Personne> Personnes { get; set; }

            private Personne personneEnCours;

            public MainWindow()
            {
                InitializeComponent();
                Personnes = new ObservableCollection<Personne>();
                ChargerDonneesDepuisFichier();
                ResetPersonneEnCours();
                datagridInfos.ItemsSource = Personnes;
            }

        private void SupprimerSelection_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = datagridInfos.SelectedItems.Cast<Personne>().ToList();
            if (selectedItems.Count > 0)
            {
                foreach (var item in selectedItems)
                {
                    Personnes.Remove(item);
                }

                // Après la suppression, mettez à jour le fichier texte.
                SauvegarderCollectionDansFichier();
            }
        }

        private void SauvegarderCollectionDansFichier()
        {
            var cheminFichier = getPath();
            // Utilisez StringBuilder pour créer le contenu du fichier.
            var sb = new StringBuilder();
            foreach (var personne in Personnes)
            {
                sb.AppendLine($"{personne.Prenom};{personne.Nom};{personne.Naissance};{personne.Promo}");
            }

            // Écriture dans le fichier en utilisant WriteAllText pour remplacer le contenu existant.
            File.WriteAllText(cheminFichier, sb.ToString());
        }

        private void ChargerDonneesDepuisFichier()
            {
            var cheminFichier = getPath();
                if (File.Exists(cheminFichier))
                {
                    using (var myFile = File.OpenText(cheminFichier))
                    {
                        string txtContent;
                        do
                        {
                            txtContent = myFile.ReadLine();
                            if (txtContent != null)
                            {
                                var elements = txtContent.Split(';');
                                if (elements.Length == 4)
                                {
                                    var personne = new Personne
                                    {
                                        Prenom = elements[0],
                                        Nom = elements[1],
                                        Naissance = elements[2],
                                        Promo = elements[3]
                                    };
                                    Personnes.Add(personne);
                                }
                            }
                        } while (txtContent != null);
                    }
                }
            }
        private string getPath()
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory; // obtient le chemin du répertoire de l'application courante
            return System.IO.Path.Combine(path, "data.txt"); // combine le chemin avec le nom du fichier
        }

        private void AjouterPersonneAuFichier(Personne personne)
            {
                var cheminFichier = getPath();
                var ligne = $"{personne.Prenom};{personne.Nom};{personne.Naissance};{personne.Promo}\n";

                // Assurez-vous que le dossier Resource existe. Sinon, créez-le.
                var dossier = Path.GetDirectoryName(cheminFichier);
                if (!Directory.Exists(dossier))
                {
                    Directory.CreateDirectory(dossier);
                }

                // Append la nouvelle personne dans le fichier
                File.AppendAllText(cheminFichier, ligne);
            }

            private void Ajouter_Click(object sender, RoutedEventArgs e)
            {
                // Ajoute la personne actuelle à la collection
                Personnes.Add(personneEnCours);

                // Écriture dans le fichier
                AjouterPersonneAuFichier(personneEnCours);

                // Réinitialise personneEnCours pour de nouvelles entrées
                ResetPersonneEnCours();
            }


            private void ResetPersonneEnCours()
            {
                personneEnCours = new Personne();
                this.DataContext = personneEnCours; // Mise à jour du DataContext pour refléter les changements
            }
        }

        public class Personne
        {
            public string Prenom { get; set; }
            public string Nom { get; set; }
            public string Naissance { get; set; }
            public string Promo { get; set; }
        }

        // Assurez-vous que la classe Personne implémente INotifyPropertyChanged comme discuté précédemment
    }
