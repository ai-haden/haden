# NXTRobot Setup

This guide documents Bluetooth pairing and serial connectivity for a LEGO NXT brick on Linux, plus project-level smoke validation in this repository.

## Preconditions

- NXT brick is powered on and visible/discoverable.
- Linux Bluetooth stack is running (`bluetooth.service`).
- You have sudo access for `rfcomm` commands.

## 1) Verify Bluetooth Host

```bash
bluetoothctl --version
rfkill list
systemctl status bluetooth --no-pager
bluetoothctl show
```

Expected: controller is `Powered: yes`, `Pairable: yes`.

## 2) Pair the NXT Brick

```bash
bluetoothctl
power on
agent KeyboardOnly
default-agent
pairable on
scan on
```

When NXT appears (example MAC `00:16:53:17:9B:47`):

```text
pair 00:16:53:17:9B:47
trust 00:16:53:17:9B:47
info 00:16:53:17:9B:47
```

Use PIN `1234` unless the brick is configured differently.

Note: `org.bluez.Error.NotAvailable br-connection-profile-unavailable` from `bluetoothctl connect` is common for NXT and does not block RFCOMM serial usage.

## 3) Bind RFCOMM Serial Device

Try channel discovery first:

```bash
sdptool browse 00:16:53:17:9B:47 | grep -E "Service Name|Channel|Serial Port" -A2
```

If discovery is empty, channel `1` is often correct for NXT SPP.

Bind `/dev/rfcomm0`:

```bash
sudo rfcomm release 0 || true
sudo rfcomm connect /dev/rfcomm0 00:16:53:17:9B:47 1
ls -l /dev/rfcomm0
```

Expected: `/dev/rfcomm0` exists (for example `crw-rw---- root dialout ...`).

## 4) User Permissions

Check your user groups:

```bash
id
```

If `dialout` is missing:

```bash
sudo usermod -aG dialout $USER
```

Then log out/in and re-check.

## 5) Project-Level Auto-Connect Smoke Test

This repo includes `Haden.HardwareSmoke` with built-in auto-connect retry.

Default run (uses `/dev/rfcomm0`):

```bash
dotnet run --project Haden.HardwareSmoke/Haden.HardwareSmoke.csproj
```

Override port via arg:

```bash
dotnet run --project Haden.HardwareSmoke/Haden.HardwareSmoke.csproj -- /dev/rfcomm0
```

Override via env vars:

```bash
export HADEN_NXT_PORT=/dev/rfcomm0
export HADEN_AUTOCONNECT_RETRIES=5
export HADEN_AUTOCONNECT_DELAY_MS=1000
dotnet run --project Haden.HardwareSmoke/Haden.HardwareSmoke.csproj
```

Expected output sequence:

- `Haden hardware smoke starting...`
- `Connected to NXT. Battery mV: ...`
- `Disconnect clean.`

## 6) Troubleshooting

- Pair succeeds but no connect via `bluetoothctl`:
  - Use `rfcomm connect` path (NXT is classic SPP).
- `/dev/rfcomm0` exists but app cannot open:
  - Check user is in `dialout`.
- Intermittent connect failures:
  - Re-run `rfcomm release 0` and `rfcomm connect ...`.
  - Ensure brick remains awake/visible during pairing and first connect.
