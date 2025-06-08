using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
	25.02.2021
*/
namespace Mkey
{
	public class OutText : MonoBehaviour
	{
		#region temp vars
		private TMP_Text text;
        #endregion temp vars

		public void IntToText(int val)
        {
			SetText(val.ToString());
        }

		private void SetText(string newText)
        {
			if (!text) text = GetComponent<TMP_Text>();
			if (text) text.text = newText;
        }
	}
}
