using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KillerSudokuSolver
{
    public class NumberSet : IEnumerable<int>, IEquatable<NumberSet>
    {
        public List<int> collection;

        public NumberSet(List<int> sourceCollection) {
            collection = new List<int>(sourceCollection);
        }

        public NumberSet(params int[] sourceCollection) {
            collection = new List<int>(sourceCollection);
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder("[ ");
            foreach(int num in collection) {
                sb.Append($"{num} ");
            }
            sb.Append($"]");
            return sb.ToString();
        }

        public IEnumerator<int> GetEnumerator() {
            
            return collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return collection.GetEnumerator();
        }

        public int GetSum() {
            int sum = 0;
            foreach(int i in collection) {
                sum += i;
            }
            return sum;
        }

        public bool HasCommonValuesWithSet(NumberSet other) {
            foreach(int i in collection) {
                if (other.ContainsNumber(i)) return true;
            }
            return false;
        }

        public bool ContainsNumber(int number) {
            return collection.Contains(number);
        }

        public bool Equals(NumberSet other) {
            if (other == null) return false;
            return this.collection.Count == other.collection.Count && this.collection.First() == other.collection.First() && this.collection.Last() == other.collection.Last() && this.GetSum() == other.GetSum();
        }
    }
}
