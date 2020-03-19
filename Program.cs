using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;

namespace KNN
{
    class Program
    {
        //calculate the distance
        static double Euclidean_Distance(string p1, string p2)
        {
            var p1data = p1.Split(";");
            var p2data = p2.Split(";");

            double sum = 0;

            for (int i = 0; i < p1data.Length - 1; i++)
            {
                sum += Math.Pow(Double.Parse(p1data[i]) - Double.Parse(p2data[i]), 2);
            }
            
            return Math.Sqrt(sum);
        }
        
        static string KnnClassification(string test, IEnumerable<string> trainingSet, int k)
        {
            return KnnTesting(test + "; ", trainingSet, k);
        } 

        static string KnnTesting(string test, IEnumerable<string> trainingSet, int k) 
        {
            string res = test.Split(";")[test.Split(";").Length - 1];
           
            List<KeyValuePair<double,string>> distances = new List<KeyValuePair<double, string>>();
            //for each row in data
            foreach (var row in trainingSet)
            {
                string group = row.Split(";")[row.Split(";").Length - 1];
                
                //calculate the distance
                var distance = Euclidean_Distance(test, row);
                
                //Add distance and the index to an ordered collection
                distances.Add(new KeyValuePair<double, string>(distance, group));

            }
            
            //then
            //Sort the ordered collection from smallest to largest
            var sortedDistances = distances.OrderBy(x => x.Key).ToList();


            //Pick the first k entries
            var kNearestNeighbours = new List<KeyValuePair<double, string>>();

            
            for (int i = 0; i < k; i++)
            {
                kNearestNeighbours.Add(sortedDistances[i]);
            }
            
            
            //Get labels of selected k entries
            List<string> results = new List<string>();
            List<int> resultsCount = new List<int>();
            foreach (var neighbour in kNearestNeighbours)
            {
                // Console.WriteLine($"{neighbour.Value} [{neighbour.Key}]");
                if (results.Contains(neighbour.Value))
                {
                    resultsCount[results.IndexOf(neighbour.Value)] += 1;
                }
                else
                {
                    results.Add(neighbour.Value);
                    resultsCount.Add(0);
                }
            }

            int max = 0;
            foreach (var count in resultsCount)
            {
                if (count > max)
                {
                    max = count;
                }
            }
            
            // Console.WriteLine($"Your result: {results[resultsCount.IndexOf(max)]}");
            // Console.WriteLine($"Result: {res}");

            return results[resultsCount.IndexOf(max)];
        } 
        
        
        static void Main(string[] args)
        {
            //Load the data
            // var trainingSetPath = @"train-set.csv";
            // var testingSetPath = @"test-set.csv";
            var trainingSetPath = @"train.csv";
            var testingSetPath = @"test.csv";
            
            var trainingSet = File.ReadLines(trainingSetPath).ToList();
            var testingSet = File.ReadLines(testingSetPath).ToList();

            //Initialize k to a chosen number
            int k = 8;
            
            
            
            // for (k = 1; k < 47; k++)
            // {
                double accurate = 0;
                int numOfTest = 0;
            
                foreach (var row in testingSet)
                {
                    string res = row.Split(";")[row.Split(";").Length - 1];
                    numOfTest += 1;
                    if (KnnTesting(row, trainingSet, k).Equals(res))
                    {
                        accurate += 1;
                    }
                }
            
                Console.WriteLine($"For k={k} accuracy is: {accurate/numOfTest}"); 
            // }

            

            while (true)    
            {
                try
                {
                    Console.WriteLine("1. sepal length in cm");
                    var sl = Double.Parse(Console.ReadLine());
                    Console.WriteLine("2. sepal width in cm");
                    var sw = Double.Parse(Console.ReadLine());
                    Console.WriteLine("3. petal length in cm");
                    var pl = Double.Parse(Console.ReadLine());
                    Console.WriteLine("4. petal width in cm");
                    var pw = Double.Parse(Console.ReadLine());
            
                    Console.WriteLine($"Your result: {KnnClassification($"{sl};{sw};{pl};{pw}", trainingSet, k)}");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Podano błędne dane!");
                }
            }









        }
    }
}