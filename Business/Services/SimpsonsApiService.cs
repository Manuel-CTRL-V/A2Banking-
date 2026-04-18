using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using ATM.Shared.DTOs.BackOffice;
using Newtonsoft.Json;

namespace BankAPI.Business.Services
{
    /// <summary>
    /// Consume la Simpsons API para validar que el titular
    /// de una cuenta sea un personaje real.
    /// Solo se usa al abrir una cuenta nueva desde el BackOffice.
    ///
    /// Endpoint: GET https://thesimpsonsapi.com/api/characters
    /// </summary>
    public class SimpsonsApiService
    {
        private static readonly HttpClient _http = new HttpClient
        {
            BaseAddress = new Uri("https://thesimpsonsapi.com/"),
            Timeout = TimeSpan.FromSeconds(10)
        };

        // Clases para deserializar la respuesta paginada
        private class PagedResponse
        {
            [JsonProperty("count")]
            public int count { get; set; }

            [JsonProperty("next")]
            public string? next { get; set; }

            [JsonProperty("prev")]
            public string? prev { get; set; }

            [JsonProperty("results")]
            public List<RawCharacter>? results { get; set; }
        }

        private class RawCharacter
        {
            public int id { get; set; }
            public string? name { get; set; }
            public int? age { get; set; }
            public string? portrait_path { get; set; }
        }

        // Obtiene TODOS los personajes recorriendo la paginación
        public List<SimpsonsCharacterDto> GetAll()
        {
            var result = new List<SimpsonsCharacterDto>();

            string url = "api/characters";

            while (!string.IsNullOrEmpty(url))
            {
                var response = _http.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();

                var json = response.Content.ReadAsStringAsync().Result;
                var page = JsonConvert.DeserializeObject<PagedResponse>(json);

                if (page?.results == null) break;

                foreach (var c in page.results)
                {
                    result.Add(new SimpsonsCharacterDto
                    {
                        CharacterId = c.id,
                        Name = c.name,
                        Age = c.age ?? 0,
                        PortraitPath = c.portrait_path
                    });
                }

                url = page.next ?? string.Empty;

                // 🔥 importante (igual que antes)
                if (!string.IsNullOrEmpty(url) && url.StartsWith("http"))
                {
                    var uri = new Uri(url);
                    url = uri.PathAndQuery.TrimStart('/');
                }
            }

            return result;
        }
        public List<SimpsonsCharacterDto> Search(string term)
        {
            var all = GetAll();
            var result = new List<SimpsonsCharacterDto>();

            term = term?.Trim() ?? string.Empty;

            foreach (var c in all)
            {
                bool matchesName = !string.IsNullOrWhiteSpace(term) &&
                    c.Name.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0;

                bool matchesId = int.TryParse(term, out int id) &&
                    c.CharacterId == id;

                if (matchesName || matchesId)
                    result.Add(c);
            }

            return result;
        }
        public bool Exists(int apiCharacterId)
        {
            foreach (var c in GetAll())
                if (c.CharacterId == apiCharacterId)
                    return true;
            return false;
        }
    }
}
