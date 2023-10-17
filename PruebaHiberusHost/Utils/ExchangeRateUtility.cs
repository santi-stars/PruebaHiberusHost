using Microsoft.AspNetCore.Mvc;
using PruebaHiberusHost.Services;
using System.Collections.ObjectModel;

namespace PruebaHiberusHost.Utils
{
    public class ExchangeRateUtility : IExchangeRateUtility
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TransactionService> _logger;
        private Dictionary<string, List<(string, double)>> graph = new Dictionary<string, List<(string, double)>>();

        public ExchangeRateUtility(ApplicationDbContext context,
                                   ILogger<TransactionService> logger)
        {
            _context = context;
            _logger = logger;
        }


        /// <summary>
        /// Calcula el tipo de cambio entre dos monedas.
        /// </summary>
        /// <param name="startCurrency">La moneda de inicio.</param>
        /// <param name="endCurrency">La moneda de destino.</param>
        /// <returns>El tipo de cambio calculado.</returns>
        public decimal CalculateExchangeRate(string startCurrency, string endCurrency)
        {
            try
            {
                _logger.LogInformation($"CALCULATE startCurrency: {startCurrency} to endCurrency: {endCurrency} RATE");
                double totalRate = 1;

                // Obtener tasas de cambio del contexto
                var exchangeRatesArray = _context.ExchangeRates.ToArray();

                Dictionary<Tuple<string, string>, double> allRatesDictionary = new Dictionary<Tuple<string, string>, double>();

                // Cargar el diccionario con las tasas de cambio y su inversa
                foreach (var rateEntity in exchangeRatesArray)
                {
                    var rateKey = Tuple.Create(rateEntity.FromCurrency, rateEntity.ToCurrency);
                    allRatesDictionary[rateKey] = (double)rateEntity.Rate;
                    rateKey = Tuple.Create(rateEntity.ToCurrency, rateEntity.FromCurrency);
                    allRatesDictionary[rateKey] = 1.0 / (double)rateEntity.Rate;
                }

                // Cargar el grafo con las tasas de cambio
                foreach (var rateDict in allRatesDictionary)
                {
                    AddEdge(rateDict.Key.Item1, rateDict.Key.Item2, rateDict.Value);
                }

                // Buscar el camino más corto para la conversión
                List<string> path = DijkstraShortestPath(startCurrency, endCurrency);

                if (path == null || path.Count == 0)
                {
                    _logger.LogError($"No se encontró un camino de conversión entre {startCurrency} y {endCurrency}");
                    throw new InvalidOperationException($"No se puede calcular el tipo de cambio entre {startCurrency} y {endCurrency}");
                }

                // Calcular el tipo de cambio total a partir del camino encontrado
                for (int j = 0; j < path.Count - 1; j++)
                {
                    Tuple<string, string> claveBusqueda = Tuple.Create(path[j], path[j + 1]);
                    if (allRatesDictionary.TryGetValue(claveBusqueda, out double valorEncontrado))
                    {
                        totalRate *= valorEncontrado;
                        _logger.LogInformation($"Valor para la rateKey ({path[j]}, {path[j + 1]}) es: {valorEncontrado}. TotalRate: {totalRate}");
                    }
                    else
                    {
                        _logger.LogWarning($"No se encontró un valor para la rateKey ({path[j]}, {path[j + 1]}).");
                    }
                }

                return (decimal)totalRate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al calcular el tipo de cambio");
                throw;
            }
        }

        /// <summary>
        /// Agrega una arista con peso entre dos monedas al grafo ponderado.
        /// </summary>
        /// <param name="fromCurrency">La primera moneda en la arista.</param>
        /// <param name="toCurrency">La segunda moneda en la arista.</param>
        /// <param name="rate">La tasa de cambio entre las dos monedas.</param>
        private void AddEdge(string fromCurrency, string toCurrency, double rate)
        {
            // Si la moneda 1 no está en el grafo, se agrega con una lista de aristas vacía.
            if (!graph.ContainsKey(fromCurrency))
            {
                graph[fromCurrency] = new List<(string, double)>();
            }

            // Se agrega la arista (toCurrency, rate) a la lista de aristas de fromCurrency.
            graph[fromCurrency].Add((toCurrency, rate));
        }

        /// <summary>
        /// Encuentra el camino más corto entre dos monedas en un grafo ponderado utilizando el algoritmo de Dijkstra.
        /// </summary>
        /// <param name="startCurrency">La moneda de inicio del camino.</param>
        /// <param name="endCurrency">La moneda de destino del camino.</param>
        /// <returns>Una lista de monedas que representa el camino más corto desde la moneda de inicio a la moneda de destino, o null si no hay un camino válido.</returns>
        private List<string> DijkstraShortestPath(string startCurrency, string endCurrency)
        {
            // Inicializa diccionarios para almacenar el costo y el nodo anterior de cada moneda.
            Dictionary<string, double> cost = new Dictionary<string, double>();
            Dictionary<string, string> previous = new Dictionary<string, string>();

            // Inicializa una lista de nodos no visitados.
            List<string> unvisited = new List<string>();

            // Inicializa el costo de todas las monedas como infinito y el nodo anterior como nulo,
            // excepto para la moneda de inicio, cuyo costo se establece en 0.
            foreach (var node in graph.Keys)
            {
                if (node == startCurrency)
                    cost[node] = 0;
                else
                    cost[node] = double.PositiveInfinity;

                previous[node] = null;
                unvisited.Add(node);
            }

            // Itera mientras queden nodos no visitados.
            while (unvisited.Count > 0)
            {
                // Ordena los nodos no visitados por el costo actual.
                unvisited.Sort((a, b) => cost[a].CompareTo(cost[b]));

                // Obtiene el nodo con el costo más bajo.
                string current = unvisited[0];
                unvisited.Remove(current);

                // Si el nodo actual es igual al nodo de destino, reconstruye y devuelve el camino más corto.
                if (current == endCurrency)
                {
                    List<string> path = new List<string>();
                    while (previous[current] != null)
                    {
                        path.Insert(0, current);
                        current = previous[current];
                    }
                    path.Insert(0, startCurrency);
                    return path;
                }

                // Si el costo del nodo actual es infinito, se detiene el proceso.
                if (cost[current] == double.PositiveInfinity)
                    break;

                // Explora los nodos vecinos y actualiza el costo si se encuentra un camino más corto.
                foreach (var neighbor in graph[current])
                {
                    double alt = cost[current] + neighbor.Item2;
                    if (alt < cost[neighbor.Item1])
                    {
                        cost[neighbor.Item1] = alt;
                        previous[neighbor.Item1] = current;
                    }
                }
            }
            // Si no se encuentra un camino, devuelve una liista vacia.
            return null;
        }

        ///// <summary>
        ///// Calcula el tipo de cambio entre dos monedas.
        ///// </summary>
        ///// <param name="startCurrency">La moneda de inicio.</param>
        ///// <param name="endCurrency">La moneda de destino.</param>
        ///// <returns>El tipo de cambio calculado.</returns>
        //public decimal CalculateExchangeRate(string startCurrency, string endCurrency)
        //{
        //    _logger.LogInformation("CALCULATE startCurrency: " + startCurrency + " to endCurrency: " + endCurrency + " RATE");
        //    double totalRate = 1;
        //    var exchangeRatesArray = _context.ExchangeRates.ToArray();   // cargar la matriz inicial con la tabla
        //    // Crear un diccionario donde la rateKey son los campos "FromCurrency" y "ToCurrency" y el valor "Rate"
        //    Dictionary<Tuple<string, string>, double> allRatesDictionary = new Dictionary<Tuple<string, string>, double>();
        //    // Carga el diccionario con todas las combinaciones posibles de PARES de MONEDAS y su RATIO y su INVERSA
        //    foreach (var rateEntity in exchangeRatesArray)
        //    {
        //        Console.WriteLine($"FromCurrency: {rateEntity.FromCurrency}, ToCurrency: {rateEntity.ToCurrency} Rate: {rateEntity.Rate}");   // BORRAR ######
        //        var rateKey = Tuple.Create(rateEntity.FromCurrency, rateEntity.ToCurrency);
        //        allRatesDictionary[rateKey] = (double)rateEntity.Rate;
        //        rateKey = Tuple.Create(rateEntity.ToCurrency, rateEntity.FromCurrency);
        //        allRatesDictionary[rateKey] = 1.0 / (double)rateEntity.Rate;
        //    }
        //    // Carga el diccionario "grafh" con la clave de peso la primera columna/moneda
        //    foreach (var rateDict in allRatesDictionary)
        //    {
        //        Console.WriteLine($"{rateDict.Key.Item1} - {rateDict.Key.Item2}: {rateDict.Value}");
        //        AddEdge(rateDict.Key.Item1, rateDict.Key.Item2, rateDict.Value);
        //    }
        //    // Crea una lista con el mínimo posible de pares de monedas para hacer la conversión
        //    List<string> path = DijkstraShortestPath(startCurrency, endCurrency);

        //    for (int j = 0; j < path.Count - 1; j++)
        //    {
        //        Tuple<string, string> claveBusqueda = Tuple.Create(path[j], path[j + 1]);

        //        if (allRatesDictionary.TryGetValue(claveBusqueda, out double valorEncontrado))
        //        {
        //            totalRate *= valorEncontrado;
        //            Console.WriteLine($"El valor para la rateKey ({path[j]}, {path[j + 1]}) es: {valorEncontrado}");
        //            Console.WriteLine(totalRate);
        //        }
        //        else
        //            Console.WriteLine($"No se encontró un valor para la rateKey ({path[j]}, {path[j + 1]}).");
        //    }
        //    Console.WriteLine("totalRate: " + totalRate);
        //    return (decimal)totalRate;
        //}
    }
}


