using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

namespace RELIVE.UI
{
	public class FileMenu : MonoBehaviour
	{
		// Perams
		[SerializeField] float fEntrySize;
		[Space]
		[SerializeField] RectTransform tFiles;
		[Space]
		[SerializeField] Sprite spFolder;
		[SerializeField] Sprite spFile;
		[SerializeField] GameObject pfFileEntry;

		// Public Data
		string m_location;
		public string location { get => m_location; set { m_location = value.Replace('\\', '/'); Refresh(); } }
		public int iNumOfEntries { get => iNumOfFolders + iNumOfFiles; }
		public int iNumOfFolders;
		public int iNumOfFiles;

		void Awake()
		{
			location = Application.dataPath;
			Refresh();
		}

		public void Refresh()
		{
			foreach (Transform entry in tFiles) Destroy(entry.gameObject);

			foreach (var folder in Directory.GetDirectories(m_location))
			{
				var entry = Instantiate(pfFileEntry, tFiles).transform;
				var entryName = folder.Split('\\').Last();

				entry.GetChild(0).GetComponent<TMP_Text>().text = entryName;
				entry.GetChild(1).GetComponent<Image>().sprite = spFolder;

				entry.GetComponent<Button>().onClick.AddListener(() => location = $"{m_location}/{entryName}");

				iNumOfFolders++;
			}

			foreach (var file in Directory.GetFiles(m_location))
			{
				var entry = Instantiate(pfFileEntry, tFiles).transform;
				var entryName = file.Split('\\').Last();

				entry.GetChild(0).GetComponent<TMP_Text>().text = entryName;
				entry.GetChild(1).GetComponent<Image>().sprite = spFile;

				iNumOfFiles++;
			}

			tFiles.sizeDelta = new Vector2(tFiles.sizeDelta.x, fEntrySize * iNumOfEntries);
		}

		public void GoBack()
		{
			location = new DirectoryInfo(m_location).Parent.FullName;
		}
	}
}