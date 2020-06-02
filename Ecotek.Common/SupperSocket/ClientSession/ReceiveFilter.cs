using SuperSocket.Common;
using SuperSocket.Facility.Protocol;
using SuperSocket.SocketBase.Protocol;
using System;

namespace Ecotek.Common.SupperSocket
{
	/// <summary>
	/// 세션으로부터 오는 리시브를 처리하기위한 클래스.
	/// 기존의 리시브필터가 유니코드 처리가 되지 않기 때문이 필요하다
	/// </summary>
	class ReceiveFilter : FixedHeaderReceiveFilter<BinaryRequestInfo>
    {
		public ReceiveFilter() : base(6)
		{
		}

		protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
		{
			return (int)header[offset + 4] * 256 + (int)header[offset + 5];
		}

		protected override BinaryRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
		{
			return new BinaryRequestInfo(SocketUtile.Inst.ByteToString(header.Array, header.Offset, 4), bodyBuffer.CloneRange(offset, length));
		}

		/// <summary>
		/// 메시지가 정확한지 판단.
		/// </summary>
		public override BinaryRequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
		{
			rest = 0;

			//데이터가 있는지?
			if (null == readBuffer)
			{
				return NullRequestInfo;
			}

			//데이터가 있다.

			//리턴할 객체
			BinaryRequestInfo briReturn;
			//원본 데이터
			DataOriginal insData = new DataOriginal();
			insData.Length = length;

			//데이터를 오프셋을 기준으로 자른다.
			Buffer.BlockCopy(readBuffer, offset, insData.Data, 0, length);

			//데이터를 넣는다.
			briReturn = new BinaryRequestInfo("", insData.Data);

			return briReturn;
		}
	}
}
