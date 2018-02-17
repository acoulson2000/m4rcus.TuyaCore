#!/bin/sh
#
set -e

NAME=TuyaService

start() {
        echo -n "Starting daemon: "$NAME
        /usr/local/bin/dotnet /opt/TuyaService/TuyaService.dll > /var/log/TuyaService.log 2>&1 &
        echo
        return 1
}

stop() {
        echo -n "Stopping daemon: "$NAME
        pkill dotnet
        echo
        return 2
}

case "$1" in
  start)
        start
  ;;
  stop)
        stop
  ;;
  restart)
        stop
        start
  ;;
  *)
        echo "Usage: "$1" {start|stop|restart}"
        exit 1
esac

exit 0

