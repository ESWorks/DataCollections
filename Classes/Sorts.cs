using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollections.Sort
{
    public static class Sorts
    {
        public static void BubbleSort<T>(this T[] value) where T : IComparable<T>
        {
            bool didASwap = false;
            do
            {
                for (int currentIndex = 0; currentIndex < (value.Length - 1); currentIndex++)
                {
                    if (value[currentIndex].CompareTo(value[currentIndex + 1]) > 0)
                    {
                        T temp = value[currentIndex];
                        value[currentIndex] = value[currentIndex + 1];
                        value[currentIndex + 1] = temp;
                        didASwap = true;
                    }
                }
            } while (didASwap);

        }

        public static void InsertionSort<T>(this T[] value) where T : IComparable<T>
        {
            for (int unsortedListStartIndex = 1; unsortedListStartIndex < value.Length; unsortedListStartIndex++)
            {
                //unsortedListStartIndex is the position at which the next unsorted item is initially found. This also
                // serves as our divide between the sorted and unsorted portions of the array.

                //CurrentUnsortedItemIndex is the current position in the list of the item we're inserting into the sorted list
                int currentUnsortedItemIndex = unsortedListStartIndex;

                //If we can still shift the "unsorted" item further to the left in the sorted section, and 
                //it should be shifted left (ie it's less than the item to its left), then shift it left
                while (currentUnsortedItemIndex - 1 >= 0 &&
                    value[currentUnsortedItemIndex].CompareTo(value[currentUnsortedItemIndex - 1]) < 0)
                {
                    T temp = value[currentUnsortedItemIndex];
                    value[currentUnsortedItemIndex] = value[currentUnsortedItemIndex - 1];
                    value[currentUnsortedItemIndex - 1] = temp;
                    currentUnsortedItemIndex--;
                }
            }
        }

        public static void SelectionSort<T>(this T[] value) where T : IComparable<T>
        {
            int Compare = 0;
            int swap = 0;
            for (int i = 0; i < value.Length - 1; i++)
            {
                int min = i;
                for (int j = i + 1; j < value.Length; j++)
                {

                    Compare++;
                    if (value[j].CompareTo(value[min]) < 0)
                    {
                        min = j;
                        swap++;
                    }

                }
                T temp = value[i];
                value[i] = value[min];
                value[min] = temp;
            }
        }

        public static void RecursiveSort(this int[] value)
        {
            RecursiveSortFunc(value, 0, value.Length - 1);
        }

        private static void RecursiveSortFunc(this int[] value, int left, int right)
        {
            //set left and right indexes
            int i = left, j = right;
            //find the pivot point in the middle of the two indexes
            int pivotIndex = left + (right - left) / 2;
            int pivot = value[pivotIndex];

            // while the left index is less than and not
            //equal to right index
            while (i <= j)
            {
                // while the left index has a value that is less than the pivot
                while (value[i].CompareTo(pivot) < 0)
                {
                    i++;
                }
                // while the right index has a value that is greater than the pivot
                while (value[j].CompareTo(pivot) > 0)
                {
                    j--;
                }
                //swap the the left most and right most values
                // if the left is less than the right
                if (i <= j)
                {
                    int temp = value[i];
                    value[i] = value[j - 1];
                    value[j - 1] = temp;
                    //increase then decrease the values
                    i++;
                    j--;
                }
            }
            //if the left is less than the rightIndex
            if (left < j)
            {
                //recurse back up
                //set array
                // set the left side index
                // set the right side index
                RecursiveSortFunc(value, left, j);
            }
            //if the leftIndex is less than the right
            if (i < right)
            {
                //recurse back up
                //set array
                // set the left side index
                // set the right side index
                RecursiveSortFunc(value, i, right);
            }

        }

        public static void ShellSort(this int[] value)
        {
            //For each gap from a large number to 1
            //for each sub-list, sort with an insertion sort
            foreach (int gap in GetGapList(value.Length))
            {
                for (int subListOffset = 0; subListOffset < gap; subListOffset++)
                {
                    InsertionSortWithGap(value, subListOffset, gap);
                }
            }
        }
        private static void InsertionSortWithGap(this int[] value, int startIndex, int gap)
        {
            //Split the array into sorted and unsorted portions. Start the sorted portion out with the left most element
            //Take the next item from the unsorted portion
            //Shift it left by swapping until it's in the correct position
            //When there are no more items in the unsorted portion, we're done

            for (int unsortedListStartIndex = startIndex + gap; unsortedListStartIndex < value.Length; unsortedListStartIndex += gap)
            {
                //unsortedListStartIndex is the position at which the next unsorted item is initially found. This also
                // serves as our divide between the sorted and unsorted portions of the array.

                //CurrentUnsortedItemIndex is the current position in the list of the item we're inserting into the sorted list
                int currentUnsortedItemIndex = unsortedListStartIndex;

                //If we can still shift the "unsorted" item further to the left in the sorted section, and 
                //it should be shifted left (ie it's less than the item to its left), then shift it left
                while (currentUnsortedItemIndex - gap >= 0 &&
                    value[currentUnsortedItemIndex].CompareTo(value[currentUnsortedItemIndex - gap]) < 0)
                {
                    int temp = value[currentUnsortedItemIndex];
                    value[currentUnsortedItemIndex] = value[currentUnsortedItemIndex - gap];
                    value[currentUnsortedItemIndex - gap] = temp;
                    currentUnsortedItemIndex -= gap;
                }
            }
        }
        private static IEnumerable<int> GetGapList(int arraySize)
        {
            Stack<int> gaps = new Stack<int>();

            int gap = 1;
            int gapNumber = 1;
            gap = (pow(3, gapNumber) - 1) / 2;
            while (gap < arraySize / 3)
            {
                gaps.Push(gap);
                gapNumber++;
                gap = (pow(3, gapNumber) - 1) / 2;
            }
            return gaps;
        }

        private static int pow(int num, int exp)
        {
            int rval = num;
            for (int i = 1; i < exp; i++)
            {
                rval *= num;
            }
            return rval;
        }
    }
}
