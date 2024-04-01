using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KillerSudokuSolver
{
    public static class Program
    {
        static void Main(string[] args) {
            bool terminate = false;
            while (!terminate) {
                int mode = RequestInteger("Enter Mode (0 = Standard, 1 = Exclusive Cross Ref)");
                if (mode <= 0) {

                    var sets = RequestSetQuery();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    foreach (NumberSet set in sets) {
                        //if (set.collection.Count < valueCount) continue;
                        Log($"Found set: {set.ToString()} totaling: {set.GetSum()}");

                    }
                } else if (mode == 1) {
                    int setCount = RequestInteger("Enter total set count. (2 or 3)");
                    List<List<NumberSet>> referenceSets = new List<List<NumberSet>>();
                    for (int setNumber = 1; setNumber <= setCount; setNumber++) {
                        var set = RequestSetQuery();
                        Log($"Got set #{setNumber}.");
                        referenceSets.Add(set);
                    }
                    if (setCount == 2) {
                        var compatibles = GetExclusivelyCompatibleSets(referenceSets[0], referenceSets[1]);
                        for (int index = 0; index < compatibles.Item1.Count; index++) {
                            Log($"Found compatible sets: {compatibles.Item1[index].ToString()} and {compatibles.Item2[index].ToString()}");
                        }

                    } else if (setCount == 3) {
                        var compatibles = GetExclusivelyCompatibleSets(referenceSets[0], referenceSets[1], referenceSets[2]);
                        for (int index = 0; index < compatibles.Item1.Count; index++) {
                            Log($"Found compatible sets: {compatibles.Item1[index].ToString()} and {compatibles.Item2[index].ToString()}  and {compatibles.Item3[index].ToString()}");
                        }
                    } else if (setCount == 4) {
                        var compatibles = GetExclusivelyCompatibleSets(referenceSets[0], referenceSets[1], referenceSets[2], referenceSets[3]);
                        for (int index = 0; index < compatibles.Item1.Count; index++) {
                            Log($"Found compatible sets: {compatibles.Item1[index].ToString()} and {compatibles.Item2[index].ToString()}  and {compatibles.Item3[index].ToString()} and {compatibles.Item4[index].ToString()}");
                        }
                    }

                    //Log($"Found set: {set.ToString()} totaling: {sum}");
                }

                Console.ForegroundColor = ConsoleColor.White;
                //Console.ReadLine();
            }
            Console.ReadLine();
        }

        /// <summary>
        /// Returns the set of sets from each collection of sets that does not conflict with the chosen set from the opposite set.
        /// </summary>
        /// <param name="setCollections"></param>
        /// <returns></returns>
        private static (List<NumberSet>, List<NumberSet>) GetExclusivelyCompatibleSets(List<NumberSet> setOne, List<NumberSet> setTwo) {
            List<NumberSet> compatibleSetsFromFirstCollection = new List<NumberSet>();
            List<NumberSet> compatibleSetsFromSecondCollection = new List<NumberSet>();
            foreach (NumberSet set in setOne) {
                foreach (NumberSet comparisonSet in setTwo) {
                    if (set.HasCommonValuesWithSet(comparisonSet) == false) {

                        compatibleSetsFromFirstCollection.Add(set);
                        compatibleSetsFromSecondCollection.Add(comparisonSet);
                        Log($"Found compatible sets: {set.ToString()} and {comparisonSet.ToString()}");
                    }
                }
            }

            return (compatibleSetsFromFirstCollection, compatibleSetsFromSecondCollection);

        }

        private static (List<NumberSet>, List<NumberSet>, List<NumberSet>) GetExclusivelyCompatibleSets(List<NumberSet> setOne, List<NumberSet> setTwo, List<NumberSet> setThree) {
            List<NumberSet> compatibleSetsFromFirstCollection = new List<NumberSet>();
            List<NumberSet> compatibleSetsFromSecondCollection = new List<NumberSet>();
            List<NumberSet> compatibleSetsFromThirdCollection = new List<NumberSet>();
            foreach (NumberSet set in setOne) {
                foreach (NumberSet comparisonSet in setTwo) {
                    if (set.HasCommonValuesWithSet(comparisonSet) == false) {
                        foreach (NumberSet compSet3 in setThree) {
                            if (set.HasCommonValuesWithSet(compSet3) == false && comparisonSet.HasCommonValuesWithSet(compSet3) == false) {
                                compatibleSetsFromFirstCollection.Add(set);
                                compatibleSetsFromSecondCollection.Add(comparisonSet);
                                compatibleSetsFromThirdCollection.Add(compSet3);
                                Log($"Found compatible sets: {set.ToString()} and {comparisonSet.ToString()} and {compSet3.ToString()}");
                            }
                        }

                    }
                }
            }

            return (compatibleSetsFromFirstCollection, compatibleSetsFromSecondCollection, compatibleSetsFromThirdCollection);

        }

        private static (List<NumberSet>, List<NumberSet>, List<NumberSet>, List<NumberSet>) GetExclusivelyCompatibleSets(List<NumberSet> setOne, List<NumberSet> setTwo, List<NumberSet> setThree, List<NumberSet> setFour) {
            List<NumberSet> compatibleSetsFromFirstCollection = new List<NumberSet>();
            List<NumberSet> compatibleSetsFromSecondCollection = new List<NumberSet>();
            List<NumberSet> compatibleSetsFromThirdCollection = new List<NumberSet>();
            List<NumberSet> compatibleSetsFromFourthCollection = new List<NumberSet>();
            foreach (NumberSet set in setOne) {
                foreach (NumberSet comparisonSet in setTwo) {
                    if (set.HasCommonValuesWithSet(comparisonSet) == false) {
                        foreach (NumberSet compSet3 in setThree) {
                            if (set.HasCommonValuesWithSet(compSet3) == false && comparisonSet.HasCommonValuesWithSet(compSet3) == false) {
                                foreach (NumberSet compSet4 in setFour) {
                                    if (set.HasCommonValuesWithSet(compSet4) == false && comparisonSet.HasCommonValuesWithSet(compSet4) == false && compSet3.HasCommonValuesWithSet(compSet4) == false) {
                                        compatibleSetsFromFirstCollection.Add(set);
                                        compatibleSetsFromSecondCollection.Add(comparisonSet);
                                        compatibleSetsFromThirdCollection.Add(compSet3);
                                        compatibleSetsFromFourthCollection.Add(compSet4);
                                        Log($"Found compatible sets: {set.ToString()} and {comparisonSet.ToString()} and {compSet3.ToString()} and {compSet4.ToString()}");
                                    }

                                }

                            }
                        }

                    }
                }
            }

            return (compatibleSetsFromFirstCollection, compatibleSetsFromSecondCollection, compatibleSetsFromThirdCollection, compatibleSetsFromFourthCollection);

        }

        private static bool DoesSetCollectionContainValue(List<NumberSet> collection, int value) {
            foreach (NumberSet set in collection) {
                if (set.ContainsNumber(value)) return true;
            }
            return false;
        }

        private static List<NumberSet> RequestSetQuery() {
            int sum = RequestInteger("Enter sum total:");
            int valueCount = RequestInteger("Enter value count: (# of boxes):");
            string[] excludedNumbers = RequestInput("Enter excluded numbers (Space separated):").Split(' ', StringSplitOptions.RemoveEmptyEntries);
            List<int> excludedNums = new List<int>();
            foreach (string s in excludedNumbers) {
                excludedNums.Add(s.AssumeInt(0));
            }
            var mySet = new NumberSet();
            var sets = CalculatePossibleSets(sum, valueCount, excludedNums);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Results completed!");
            Console.ForegroundColor = ConsoleColor.Green;
            return sets.Where((NumberSet set) => { return set.collection.Count == valueCount; }).ToList();
        }

        public static List<NumberSet> CalculatePossibleSets(int sum, int setLength, List<int> excludedNumbers = null, List<int> requiredNumbers = null) {
            // Initialize data structures
            List<NumberSet> possibleSets = new List<NumberSet>();
            List<int> currentSet = new List<int>();

            // Handle default values for excluded and required numbers
            if (excludedNumbers == null) {
                excludedNumbers = new List<int>();
            }
            if (requiredNumbers == null) {
                requiredNumbers = new List<int>();
            }

            // Recursive helper function (non-public)
            void FindSets(int remainingSum, int index) {
                // Base Case: Set is complete 
                if (currentSet.Count == setLength) {
                    if (remainingSum == 0) {
                        //new List<int>(currentSet)
                        possibleSets.Add(new NumberSet(currentSet)); // Add a copy
                    }
                    return;
                }

                // Base Case: No valid numbers remain
                if (index > 9 || remainingSum < 0) {
                    return;
                }

                // Recursive Cases:
                int num = index;

                // 1. Exclude current number
                FindSets(remainingSum, index + 1);

                // 2. Include current number (if not excluded and meets requirements)
                if (!excludedNumbers.Contains(num) &&
                    (!requiredNumbers.Any() || requiredNumbers.Contains(num))) {
                    currentSet.Add(num);
                    FindSets(remainingSum - num, index + 1);
                    currentSet.RemoveAt(currentSet.Count - 1); // Backtrack
                }
            }

            // Start the recursive process
            FindSets(sum, 1);

            return possibleSets;
        }



        /*static List<NumberSet> currentSet = new List<NumberSet>();
        private static List<NumberSet> CalculatePossibleValues(int sum, int valueCount, List<int> excludedNumbers, ref NumberSet runningSet, int iteration = 1) {
            if (iteration == 1) {
                currentSet = new List<NumberSet>();
            }
            if (sum <= 0) {
                return new List<NumberSet>();
            }
            string prepend = $"";
            for (int i = 1; i <= iteration; i++) {
                prepend += "- ";
            }
            prepend += ">";
            //Log($"{prepend} [Asking about sum of {valueCount} numbers that add to: {sum}...]");

            if (valueCount == 1) {
                if (sum.IsWithinRange(1, 9) && excludedNumbers.Contains(sum) == false) {
                    //Log($"Found valid number: {sum}");
                    runningSet.collection.Add(sum);
                    runningSet.collection.Sort();
                    var returnSet = new NumberSet(runningSet.collection.ToArray());
                    var returnVal = new List<NumberSet>() { returnSet };
                    if (currentSet.Contains(returnSet) == false) {
                        currentSet.Add(returnSet);
                    }

                    //Log($"New running set: {returnSet.ToString()}\n--> Returning: {JsonConvert.SerializeObject(returnVal) }");
                    return returnVal;
                } else {
                    return new List<NumberSet>();
                }
            } else if (valueCount > 1) {
                List<NumberSet> returnSet = new List<NumberSet>();

                for (int number = 1; number <= 9; number++) {
                    if (excludedNumbers.Contains(number)) continue;
                    if (number >= sum) continue;
                    if (runningSet.collection.Contains(number)) continue;
                    //Log($"Checking number: {number} against set: {runningSet.ToString()}");
                    var cloneList = new List<int>(excludedNumbers);
                    cloneList.Add(number);
                    NumberSet derivedSet = new NumberSet(runningSet.collection.ToArray());
                    derivedSet.collection.Add(number);
                    var result = (CalculatePossibleValues(sum - number, valueCount - 1, cloneList, ref derivedSet, iteration + 1));

                    if (iteration == 1) {
                        //Log($"Finished calculation for sub result.");
                        // Log($"Found completed set: {JsonConvert.SerializeObject(result)}\n");

                        returnSet.AddRange(result);
                        //Log($"Return set: {JsonConvert.SerializeObject(returnSet)}\n");
                    }

                }
                return currentSet;
            }
            Log($"Reached an unacceptable exit condition!");
            return new List<NumberSet>();
        }*/

        static void Log(string s) {
            Console.WriteLine(s);
        }

        static void Log(this object s) {
            Console.WriteLine(s?.ToString());
        }

        static bool IsWithinRange(this int number, int lowRange, int highRange) {
            return number >= lowRange && number <= highRange;
        }

        static string RequestInput(string prompt) {
            Console.WriteLine(prompt);
            return Console.ReadLine();
        }

        static int RequestInteger(string prompt, int defaultVal = 0) {
            Console.WriteLine(prompt);
            return ((string)Console.ReadLine()).AssumeInt(defaultVal);
        }

        public static int AssumeInt(this string s, int defaultValue = 0) {
            try {
                return Convert.ToInt32(s);
            } catch {
                return defaultValue;
            }

        }
    }
}
