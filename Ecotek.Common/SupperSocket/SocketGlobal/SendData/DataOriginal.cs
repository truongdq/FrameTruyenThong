namespace Ecotek.Common.SupperSocket
{
	public struct DataOriginal
    {
		/// <summary>
		/// Set data size (original)
		/// </summary>
		private int m_nLength;

		/// <summary>
		/// Data (original)
		/// </summary>
		private byte[] m_byteData;

		/// <summary>
		/// Set data size
		/// </summary>
		public int Length
		{
			get
			{
				return this.m_nLength;
			}
			set
			{
				//The data length is stored
				this.m_nLength = value;
				//Set the data length
				this.m_byteData = new byte[this.m_nLength];
			}
		}

		/// <summary>
		/// Data
		/// </summary>
		public byte[] Data
		{
			get
			{
				return this.m_byteData;
			}
			set
			{
				m_byteData = value;
				// Save the data
				this.m_nLength = m_byteData.Length;
			}
		}
	}
}
