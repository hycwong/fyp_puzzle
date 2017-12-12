using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;

public class FileEncrypt {
	

	public static void encrypt(string path) {

			byte[] file_byte = File.ReadAllBytes (path);

			RijndaelManaged rDel = new RijndaelManaged ();
			rDel.Key = System.Text.Encoding.UTF8.GetBytes("S7yP9DZLqaFyyO3uoMnavI1bNUDa7kQC");
			rDel.Mode = CipherMode.ECB;
			rDel.Padding = PaddingMode.PKCS7;

			ICryptoTransform cTransform = rDel.CreateEncryptor ();
			byte[] resultArray = cTransform.TransformFinalBlock (file_byte, 0, file_byte.Length);

			string s = System.Convert.ToBase64String(resultArray);
			File.WriteAllText (path, s);

	}

	public static void decrypt(string path) {

			string file_string = File.ReadAllText (path);
			byte[] b = System.Convert.FromBase64String (file_string);

			RijndaelManaged rDel = new RijndaelManaged ();
			rDel.Key = System.Text.Encoding.UTF8.GetBytes ("S7yP9DZLqaFyyO3uoMnavI1bNUDa7kQC");
			rDel.Mode = CipherMode.ECB;
			rDel.Padding = PaddingMode.PKCS7;

			ICryptoTransform cTransform = rDel.CreateDecryptor ();
			byte[] resultArray = cTransform.TransformFinalBlock (b, 0, b.Length);

			File.WriteAllBytes (path, resultArray);

	}


}
