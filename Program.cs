using System;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStoreApp
{
    class Program
    {
        static HttpClient httpClient = new HttpClient();

        static string baseUrl = "https://localhost:5001/swagger/index.html"; 

        static async Task Main(string[] args)
        {
            Console.WriteLine("Aplicație Magazin Online");

            // Ignorăm validarea certificatului
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;


            while (true)
            {
                Console.WriteLine("\nSelectați o opțiune:");
                Console.WriteLine("1. Enumerare categorii");
                Console.WriteLine("2. Detalii despre o categorie");
                Console.WriteLine("3. Creare categorie nouă");
                Console.WriteLine("4. Ștergere categorie");
                Console.WriteLine("5. Modificare titlu categorie");
                Console.WriteLine("6. Adăugare produs într-o categorie");
                Console.WriteLine("7. Listare produse dintr-o categorie");
                Console.WriteLine("0. Ieșire");
                Console.Write("> ");

                string option = Console.ReadLine();
                Console.WriteLine();

                switch (option)
                {
                    case "1":
                        await ListCategories();
                        break;
                    case "2":
                        await GetCategoryDetails();
                        break;
                    case "3":
                        await CreateCategory();
                        break;
                    case "4":
                        await DeleteCategory();
                        break;
                    case "5":
                        await UpdateCategoryTitle();
                        break;
                    case "6":
                        await AddProductToCategory();
                        break;
                    case "7":
                        await ListProductsInCategory();
                        break;
                    case "0":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Opțiune invalidă!");
                        break;
                }
            }
        }

        static async Task ListCategories()
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync($"{baseUrl}/categories");
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Lista de categorii:");
                Console.WriteLine(responseBody);
            }
            catch (Exception e)
            {
                Console.WriteLine("Eroare la enumerarea categoriilor: " + e.Message);
            }
        }

        static async Task GetCategoryDetails()
        {
            Console.Write("Introduceți ID-ul categoriei: ");
            string categoryId = Console.ReadLine();

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync($"{baseUrl}/categories/{categoryId}");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine("Categoria nu există.");
                    return;
                }

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Detalii despre categoria selectată:");
                Console.WriteLine(responseBody);
            }
            catch (Exception e)
            {
                Console.WriteLine("Eroare la obținerea detaliilor categoriei: " + e.Message);
            }
        }

        static async Task CreateCategory()
        {
            Console.Write("Introduceți titlul categoriei noi: ");
            string title = Console.ReadLine();

            try
            {
                HttpResponseMessage response = await httpClient.PostAsync($"{baseUrl}/categories", new StringContent($"{{ \"title\": \"{title}\" }}", Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();

                Console.WriteLine("Categoria a fost creată cu succes!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Eroare la crearea categoriei: " + e.Message);
            }
        }

        static async Task DeleteCategory()
        {
            Console.Write("Introduceți ID-ul categoriei de șters: ");
            string categoryId = Console.ReadLine();

            try
            {
                HttpResponseMessage response = await httpClient.DeleteAsync($"{baseUrl}/categories/{categoryId}");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine("Categoria nu există.");
                    return;
                }

                response.EnsureSuccessStatusCode();

                Console.WriteLine("Categoria a fost ștearsă cu succes!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Eroare la ștergerea categoriei: " + e.Message);
            }
        }

        static async Task UpdateCategoryTitle()
        {
            Console.Write("Introduceți ID-ul categoriei de modificat: ");
            string categoryId = Console.ReadLine();

            Console.Write("Introduceți noul titlu al categoriei: ");
            string newTitle = Console.ReadLine();

            try
            {
                HttpResponseMessage response = await httpClient.PutAsync($"{baseUrl}/categories/{categoryId}", new StringContent($"{{ \"title\": \"{newTitle}\" }}", Encoding.UTF8, "application/json"));
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine("Categoria nu există.");
                    return;
                }

                response.EnsureSuccessStatusCode();

                Console.WriteLine("Titlul categoriei a fost modificat cu succes!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Eroare la modificarea titlului categoriei: " + e.Message);
            }
        }

        static async Task AddProductToCategory()
        {
            Console.Write("Introduceți ID-ul categoriei în care se adaugă produsul: ");
            string categoryId = Console.ReadLine();

            Console.Write("Introduceți numele produsului nou: ");
            string productName = Console.ReadLine();

            Console.Write("Introduceți prețul produsului nou: ");
            decimal productPrice = Convert.ToDecimal(Console.ReadLine());

            try
            {
                HttpResponseMessage response = await httpClient.PostAsync($"{baseUrl}/categories/{categoryId}/products", new StringContent($"{{ \"name\": \"{productName}\", \"price\": {productPrice} }}", Encoding.UTF8, "application/json"));
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine("Categoria nu există.");
                    return;
                }

                response.EnsureSuccessStatusCode();

                Console.WriteLine("Produsul a fost adăugat cu succes în categoria selectată!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Eroare la adăugarea produsului: " + e.Message);
            }
        }

        static async Task ListProductsInCategory()
        {
            Console.Write("Introduceți ID-ul categoriei pentru a lista produsele: ");
            string categoryId = Console.ReadLine();

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync($"{baseUrl}/categories/{categoryId}/products");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine("Categoria nu există.");
                    return;
                }

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Lista de produse din categoria selectată:");
                Console.WriteLine(responseBody);
            }
            catch (Exception e)
            {
                Console.WriteLine("Eroare la listarea produselor: " + e.Message);
            }
        }
    }
}
