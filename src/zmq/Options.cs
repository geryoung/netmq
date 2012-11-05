/*
    Copyright (c) 2007-2012 iMatix Corporation
    Copyright (c) 2009-2011 250bpm s.r.o.
    Copyright (c) 2011 VMware, Inc.
    Copyright (c) 2007-2011 Other contributors as noted in the AUTHORS file
        
    This file is part of 0MQ.
            
    0MQ is free software; you can redistribute it and/or modify it under
    the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation; either version 3 of the License, or
    (at your option) any later version.
            
    0MQ is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.
        
    You should have received a copy of the GNU Lesser General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace zmq
{
	public class Options
	{
		public Options()
		{
			SendHighWatermark = 1000;
			ReceiveHighWatermark = 1000;
			Affinity = 0;
			IdentitySize = 0;
			Rate = 100;
			RecoveryIvl = 10000;
			MulticastHops = 1;
			SendBuffer = 0;
			ReceiveBuffer = 0;
			SocketType = ZmqSocketType.None;
			Linger = -1;
			ReconnectIvl = 100;
			ReconnectIvlMax = 0;
			Backlog = 100;
			Maxmsgsize = -1;
			ReceiveTimeout = -1;
			SendTimeout = -1;
			IPv4Only = 1;
			DelayAttachOnConnect = 0;
			DelayOnClose = true;
			DelayOnDisconnect = true;
			Filter = false;
			RecvIdentity = false;
			TcpKeepalive = -1;
			TcpKeepaliveCnt = -1;
			TcpKeepaliveIdle = -1;
			TcpKeepaliveIntvl = -1;
			SocketId = 0;

			Identity = null;
			TcpAcceptFilters = new List<TcpAddress.TcpAddressMask>();
		}

		//  High-water marks for message pipes.
		public int SendHighWatermark { get; set; }
		public int ReceiveHighWatermark { get; set; }

		//  I/O thread affinity.
		public long Affinity { get; set; }

		//  Socket identity
		public byte IdentitySize { get; set; }
		public byte[] Identity { get; set; } // [256];

		// Last socket endpoint resolved URI
		public String LastEndpoint { get; set; }

		//  Maximum tranfer rate [kb/s]. Default 100kb/s.
		public int Rate { get; set; }

		//  Reliability time interval [ms]. Default 10 seconds.
		public int RecoveryIvl { get; set; }

		// Sets the time-to-live field in every multicast packet sent.
		public int MulticastHops { get; set; }

		// SO_SNDBUF and SO_RCVBUF to be passed to underlying transport sockets.
		public int SendBuffer { get; set; }
		public int ReceiveBuffer { get; set; }

		//  Socket type.
		public ZmqSocketType SocketType { get; set; }

		//  Linger time, in milliseconds.
		public int Linger { get; set; }

		//  Minimum interval between attempts to reconnect, in milliseconds.
		//  Default 100ms
		public int ReconnectIvl { get; set; }
		//  Maximum interval between attempts to reconnect, in milliseconds.
		//  Default 0 (unused)
		public int ReconnectIvlMax { get; set; }

		//  Maximum backlog for pending connections.
		public int Backlog { get; set; }

		//  Maximal size of message to handle.
		public long Maxmsgsize { get; set; }

		// The timeout for send/recv operations for this socket.
		public int ReceiveTimeout { get; set; }
		public int SendTimeout { get; set; }

		//  If 1, indicates the use of IPv4 sockets only, it will not be
		//  possible to communicate with IPv6-only hosts. If 0, the socket can
		//  connect to and accept connections from both IPv4 and IPv6 hosts.
		public int IPv4Only { get; set; }

		//  If 1, connecting pipes are not attached immediately, meaning a send()
		//  on a socket with only connecting pipes would block
		public int DelayAttachOnConnect { get; set; }

		//  If true, session reads all the pending messages from the pipe and
		//  sends them to the network when socket is closed.
		public bool DelayOnClose { get; set; }

		//  If true, socket reads all the messages from the pipe and delivers
		//  them to the user when the peer terminates.
		public bool DelayOnDisconnect { get; set; }

		//  If 1, (X)SUB socket should filter the messages. If 0, it should not.
		public bool Filter { get; set; }

		//  If true, the identity message is forwarded to the socket.
		public bool RecvIdentity { get; set; }

		//  TCP keep-alive settings.
		//  Defaults to -1 = do not change socket options
		public int TcpKeepalive { get; set; }
		public int TcpKeepaliveCnt { get; set; }
		public int TcpKeepaliveIdle { get; set; }
		public int TcpKeepaliveIntvl { get; set; }

		// TCP accept() filters
		//typedef std::vector <tcp_address_mask_t> tcp_accept_filters_t;
		public List<TcpAddress.TcpAddressMask> TcpAcceptFilters { get; private set; }

		//  ID of the socket.
		public int SocketId { get; set; }


		public bool SetSocketOption(ZmqSocketOptions option, Object optval)
		{
			switch (option)
			{
				case ZmqSocketOptions.SendHighWatermark:
					SendHighWatermark = (int) optval;
					return true;

				case ZmqSocketOptions.ReceivevHightWatermark:
					ReceiveHighWatermark = (int) optval;
					return true;


				case ZmqSocketOptions.Affinity:
					Affinity = (long) optval;
					return true;

				case ZmqSocketOptions.Identity:
					byte[] val;

					if (optval is String)
						val = Encoding.ASCII.GetBytes((String) optval);
					else if (optval is byte[])
						val = (byte[]) optval;
					else
					{
						ZError.ErrorNumber = (ErrorNumber.EINVAL);
						return false;
					}

					if (val.Length ==0 || val.Length > 255)
					{
						ZError.ErrorNumber = (ErrorNumber.EINVAL);
						return false;
					}
					Identity = new byte[val.Length];
					val.CopyTo(Identity, 0);
					IdentitySize = (byte) Identity.Length;
					return true;

				case ZmqSocketOptions.Rate:
					Rate = (int) optval;
					return true;

				case ZmqSocketOptions.RecoveryIvl:
					RecoveryIvl = (int) optval;
					return true;

				case ZmqSocketOptions.SendBuffer:
					SendBuffer = (int) optval;
					return true;

				case ZmqSocketOptions.ReceivevBuffer:
					ReceiveBuffer = (int) optval;
					return true;

				case ZmqSocketOptions.Linger:
					Linger = (int) optval;
					return true;

				case ZmqSocketOptions.ReconnectIvl:
					ReconnectIvl = (int) optval;

					if (ReconnectIvl < -1)
					{
						ZError.ErrorNumber = (ErrorNumber.EINVAL);
						return false;
					}

					return true;

				case ZmqSocketOptions.ReconnectIvlMax:
					ReconnectIvlMax = (int) optval;

					if (ReconnectIvlMax < 0)
					{
						ZError.ErrorNumber = (ErrorNumber.EINVAL);
						return false;
					}

					return true;

				case ZmqSocketOptions.Backlog:
					Backlog = (int) optval;
					return true;


				case ZmqSocketOptions.Maxmsgsize:
					Maxmsgsize = (long) optval;
					return true;

				case ZmqSocketOptions.MulticastHops:
					MulticastHops = (int) optval;
					return true;

				case ZmqSocketOptions.ReceiveTimeout:
					ReceiveTimeout = (int) optval;
					return true;

				case ZmqSocketOptions.SendTimeout:
					SendTimeout = (int) optval;
					return true;

				case ZmqSocketOptions.IPv4Only:

					IPv4Only = (int) optval;
					if (IPv4Only != 0 && IPv4Only != 1)
					{
						ZError.ErrorNumber = (ErrorNumber.EINVAL);
						return false;
					}
					return true;

				case ZmqSocketOptions.TcpKeepalive:

					TcpKeepalive = (int) optval;
					if (TcpKeepalive != -1 && TcpKeepalive != 0 && TcpKeepalive != 1)
					{
						ZError.ErrorNumber = (ErrorNumber.EINVAL);
						return false;
					}
					return true;

				case ZmqSocketOptions.DelayAttachOnConnect:

					DelayAttachOnConnect = (int) optval;
					if (DelayAttachOnConnect != 0 && DelayAttachOnConnect != 1)
					{
						ZError.ErrorNumber = (ErrorNumber.EINVAL);
						return false;
					}
					return true;

				case ZmqSocketOptions.TcpKeepaliveCnt:
				case ZmqSocketOptions.TcpKeepaliveIdle:
				case ZmqSocketOptions.TcpKeepaliveIntvl:
					// not supported
					return true;

				case ZmqSocketOptions.TcpAcceptFilter:
					String filterStr = (String) optval;
					if (filterStr == null)
					{
						TcpAcceptFilters.Clear();
					}
					else if (filterStr.Length == 0 || filterStr.Length > 255)
					{
						ZError.ErrorNumber = (ErrorNumber.EINVAL);
						return false;
					}
					else
					{
						TcpAddress.TcpAddressMask filter = new TcpAddress.TcpAddressMask();
						filter.Resolve(filterStr, IPv4Only == 1);
						TcpAcceptFilters.Add(filter);
					}
					return true;

				default:
					ZError.ErrorNumber = (ErrorNumber.EINVAL);
					return false;
			}
		}


		public Object GetSocketOption(ZmqSocketOptions option)
		{
			switch (option)
			{
				case ZmqSocketOptions.SendHighWatermark:
					return SendHighWatermark;

				case ZmqSocketOptions.ReceivevHightWatermark:
					return ReceiveHighWatermark;

				case ZmqSocketOptions.Affinity:
					return Affinity;

				case ZmqSocketOptions.Identity:
					return Identity;

				case ZmqSocketOptions.Rate:
					return Rate;

				case ZmqSocketOptions.RecoveryIvl:
					return RecoveryIvl;

				case ZmqSocketOptions.SendBuffer:
					return SendBuffer;

				case ZmqSocketOptions.ReceivevBuffer:
					return ReceiveBuffer;

				case ZmqSocketOptions.Type:
					return SocketType;

				case ZmqSocketOptions.Linger:
					return Linger;

				case ZmqSocketOptions.ReconnectIvl:
					return ReconnectIvl;

				case ZmqSocketOptions.ReconnectIvlMax:
					return ReconnectIvlMax;

				case ZmqSocketOptions.Backlog:
					return Backlog;

				case ZmqSocketOptions.Maxmsgsize:
					return Maxmsgsize;

				case ZmqSocketOptions.MulticastHops:
					return MulticastHops;

				case ZmqSocketOptions.ReceiveTimeout:
					return ReceiveTimeout;

				case ZmqSocketOptions.SendTimeout:
					return SendTimeout;

				case ZmqSocketOptions.IPv4Only:
					return IPv4Only;

				case ZmqSocketOptions.TcpKeepalive:
					return TcpKeepalive;

				case ZmqSocketOptions.DelayAttachOnConnect:
					return DelayAttachOnConnect;

				case ZmqSocketOptions.TcpKeepaliveCnt:
				case ZmqSocketOptions.TcpKeepaliveIdle:
				case ZmqSocketOptions.TcpKeepaliveIntvl:
					// not supported
					return 0;

				case ZmqSocketOptions.LastEndpoint:
					return LastEndpoint;

				default:
					throw new ArgumentException("option=" + option);
			}
		}
	}
}