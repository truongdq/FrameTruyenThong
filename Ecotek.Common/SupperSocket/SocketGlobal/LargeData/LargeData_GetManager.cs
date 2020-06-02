using System;
using System.Collections.Generic;
using System.IO;

namespace Ecotek.Common.SupperSocket
{
	/*
	 * Large data processing is done by loading the data into memory and recording the file once the upload is completed.
     * Therefore, uploading a file that is too large puts a heavy burden on the server (I will change it to IO recording method later)
	 */
	/// <summary>
	/// Manager for handling large data
	/// </summary>
	public class LargeData_GetManager
	{
		/// <summary>
		/// Counter to be used as index of uploaded file
		/// </summary>
		public int m_nFileCount = 0;

		/// <summary>
		/// Managed data list
		/// </summary>
		private List<LargeData_GetData> m_ListGetData = new List<LargeData_GetData>();


		/// <summary>
		/// Prepares to receive data based on the information sent from the sender.
		/// </summary>
		public int Start(TypeLargeData typeLD, string sFileName, int nTotalLength, int nTotalCount, int nIndex)
		{
			++this.m_nFileCount;

			//Create an object to add to the list
			LargeData_GetData insLargeData = new LargeData_GetData();
			// Data setting
			insLargeData.SettingData(typeLD
									, sFileName
									, nTotalLength
									, nTotalCount
									, this.m_nFileCount);

			//Default route setting
			insLargeData.Dir = @"c:\";

			//Add to list
			this.m_ListGetData.Add(insLargeData);

			return this.m_nFileCount;
		}

		public TypeLargeDataManager AddData(byte[] byteData, out int nDBIndex, out int nCount)
		{
			TypeLargeDataManager typeReturn = TypeLargeDataManager.None;

			byte[] byteTemp;

			//1. DB index extraction
			byteTemp = new byte[Global.g_DataHeader1Size];
			Buffer.BlockCopy(byteData, 0, byteTemp, 0, Global.g_DataHeader1Size);
			//Temporary variables must be declared because of lambda error problems-_-;;
			int nDBIndex_Temp = SocketUtile.Inst.ByteToInt(byteTemp);
			nDBIndex = nDBIndex_Temp;

			//2. Find the object using the extracted DB index
			LargeData_GetData insLD = m_ListGetData.Find(x => x.DBIndex == nDBIndex_Temp);

			//3. Data storage
			// Do you have a target?
			if (null != insLD)
			{   //have!
				// 3-1. Extract data sequence
				byteTemp = new byte[Global.g_DataHeader2Size];
				Buffer.BlockCopy(byteData, Global.g_DataHeader1Size, byteTemp, 0, Global.g_DataHeader2Size);
				nCount = SocketUtile.Inst.ByteToInt(byteTemp);

				// 3-2. Data extract
				int nCutByteSize = -1;
				if (Global.g_CutByteSize > (insLD.Length_Total - insLD.Length_Now))
				{   // The remaining data is smaller than the cut data.
					// Cut only the remaining size.
					nCutByteSize = insLD.Length_Total - insLD.Length_Now;
				}
				else
				{   // The remaining data is more than the cut data.
					// Cut by one size.
					nCutByteSize = Global.g_CutByteSize;
				}

				byteTemp = new byte[nCutByteSize];
				Buffer.BlockCopy(byteData, Global.g_DataHeader1Size + Global.g_DataHeader1Size, byteTemp, 0, nCutByteSize);
				int nLength = byteTemp.Length;

				// 3-3. Data recording
				Buffer.BlockCopy(byteTemp, 0, insLD.Data, nCount * Global.g_CutByteSize, nCutByteSize);

				// Record status of extracted data
				insLD.Count_Now += 1;           // number of data
				insLD.Length_Now += nLength;    // data capacity

				typeReturn = TypeLargeDataManager.Complete;
			}
			else
			{   //none
				typeReturn = TypeLargeDataManager.NoItem;
				nCount = -1;
			}

			return typeReturn;
		}

		/// <summary>
		/// Ensure that all data has been transferred
		/// </summary>
		public bool CheckData(int nDBIndex)
		{
			//Find the object using the DB index.
			LargeData_GetData insLD = this.m_ListGetData.Find(x => x.DBIndex == nDBIndex);

			return CheckData(insLD);
		}

		/// <summary>
		/// Ensure that all data has been transmitted..
		/// </summary>
		/// <param name="insLD_GD"></param>
		/// <returns></returns>
		public bool CheckData(LargeData_GetData insLD_GD)
		{
			bool bReturn;
			if (insLD_GD.Count_Total <= insLD_GD.Count_Now)
			{
				bReturn = true;
			}
			else
			{
				bReturn = false;
			}

			return bReturn;
		}

		/// <summary>
		/// Handles large data received.
		/// </summary>
		public bool CompleteData(int nDBIndex)
		{
			bool bReturn = false;

			//Use DB indexes to find objects.
			LargeData_GetData insLD = this.m_ListGetData.Find(x => x.DBIndex == nDBIndex);

			//Verify that the data transfer is complete
			if (true == this.CheckData(insLD))
			{   //I received all the data..

				bReturn = true;

				switch (insLD.TypeLargeData)
				{
					case TypeLargeData.BigMessage:  //Big message
						bReturn = Processing_BigMessage(insLD);
						break;
					case TypeLargeData.File:        //File
						bReturn = Processing_File(insLD);
						break;
					default:    //This means that the default processing can not process the data.
						bReturn = false;
						break;
				}
			}

			return bReturn;
		}

		private bool Processing_BigMessage(LargeData_GetData insLD_GD)
		{
			bool bReturn = false;
			insLD_GD.DataCompleteRead = true;
			return bReturn;
		}

		/// <summary>
		/// Save saved data to file
		/// </summary>
		private bool Processing_File(LargeData_GetData insLD_GD)
		{
			bool bReturn = false;

			if (true == insLD_GD.DataCompleteRead)
			{   //It's already done..
				bReturn = true;
			}
			else
			{   //It's not done yet.
				try
				{
					//Create a file to save
					FileStream fs = new FileStream(insLD_GD.Dir + insLD_GD.FileName, FileMode.Create, FileAccess.Write);

					//save file
					BinaryWriter bw = new BinaryWriter(fs);
					bw.Write(insLD_GD.Data, 0, insLD_GD.Length_Total);
					bw.Close();

					bReturn = true;
					insLD_GD.DataCompleteRead = true;
				}
				catch
				{
					bReturn = false;
				}
			}

			return bReturn;
		}
	}
}
