using UnityEngine;
using System.Collections;
using System;

// This script is used to find the records within a certain Date range
public class DateHelper : MonoBehaviour {

	// Use this for initialization
	public int getIndexForDateWithinDays(DateTime[] date_array,DateTime startDate, int days) {

		DateTime[] subArray;
		int midpoint;

		if (date_array.Length % 2 == 0) {
			midpoint = (int) (date_array.Length / 2);
		} else {
			midpoint = (int) ((date_array.Length - 1) / 2);		
		}

		int endpoint = date_array.Length - 1;

		if (date_array.Length == 1 && (date_array[0]-startDate).TotalDays < days) {
			return 0;
		}

		if(date_array.Length == 1 && (date_array[0]-startDate).TotalDays > days) {
			return -1;
		}
	
		if ((DateTime.Now - date_array [midpoint]).TotalDays > days) {  
			
			if ((DateTime.Now - date_array [midpoint + 1]).TotalDays <= days) {
				return midpoint + 1;
			} else {
				subArray = new DateTime[date_array.Length - midpoint - 1];
				Array.Copy (date_array, midpoint + 1, subArray, 0, date_array.Length - midpoint - 1);

				print (midpoint + 1);
				print (date_array.Length - midpoint - 1);

				return midpoint + 1 + getIndexForDateWithinDays (subArray, startDate, days);
			} 

		} else if ((DateTime.Now - date_array [midpoint]).TotalDays < days) {

			if ((DateTime.Now - date_array [midpoint - 1]).TotalDays >= days) {
				return midpoint;
			} else if ((DateTime.Now - date_array [midpoint - 1]).TotalDays < days) {
				subArray = new DateTime[midpoint];
				Array.Copy (date_array, 0, subArray, 0, midpoint);
				return getIndexForDateWithinDays (subArray, startDate, days);
			} else
				return 0;

		} else
			return midpoint;
		
	}


}
