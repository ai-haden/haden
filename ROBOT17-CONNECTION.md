# ROBOT17 Bluetooth Connection

ROBOT17 is the paired LEGO NXT brick used by this repository.

| Setting | Value |
| --- | --- |
| Bluetooth address | `00:16:53:17:9B:47` |
| Bluetooth service | Serial Port Profile (SPP) |
| RFCOMM channel | `1` |

## Battery sanity check

Query the NXT directly using Python's Bluetooth RFCOMM socket. This does not
require `sudo` or creation of a `/dev/rfcomm*` device.

```bash
python3 -c "import socket; s=socket.socket(socket.AF_BLUETOOTH,socket.SOCK_STREAM,socket.BTPROTO_RFCOMM); s.settimeout(10); s.connect(('00:16:53:17:9B:47',1)); s.sendall(bytes((2,0,0,11))); data=s.recv(64); print(data.hex()); s.close()"
```

The request payload is the NXT direct-command frame for `GetBatteryLevel`
(`0x0B`). A successful response has this form:

```text
05 00 02 0b 00 LL HH
```

- `05 00`: little-endian response payload length
- `02 0b 00`: reply, `GetBatteryLevel`, success
- `LL HH`: little-endian battery voltage in millivolts

Example result recorded during the connection check:

```text
0500020b008220
```

This decodes to `0x2082`, or **8,322 mV (8.322 V)**.

The brick must already be paired with the host before running the query.
