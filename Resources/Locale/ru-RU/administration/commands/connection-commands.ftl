## Strings for the "grant_connect_bypass" command.

cmd-grant_connect_bypass-desc = Временно разрешить пользователю обходить регулярные проверки подключения.
cmd-grant_connect_bypass-help =
    Использование: grant_connect_bypass <user> [duration minutes]
    Временно предоставляет пользователю возможность обходить обычные ограничения на подключение.
    Обход действует только на этом игровом сервере и истекает (по умолчанию) через 1 час.
    Они смогут присоединиться независимо от наличия белого списка, бункера паники или шапки игрока.
cmd-grant_connect_bypass-arg-user = <user>
cmd-grant_connect_bypass-arg-duration = [duration minutes]
cmd-grant_connect_bypass-invalid-args = Ожидаемые 1 или 2 аргумента
cmd-grant_connect_bypass-unknown-user = Не удается найти пользователя '{ $user }'
cmd-grant_connect_bypass-invalid-duration = Недопустимая продолжительность '{ $duration }'
cmd-grant_connect_bypass-success = Успешно добавлен обход для пользователя '{ $user }'
