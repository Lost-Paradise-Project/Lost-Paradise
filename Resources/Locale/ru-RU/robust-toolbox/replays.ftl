# Playback Commands

cmd-replay-play-desc = Возобновите воспроизведение повтора.
cmd-replay-play-help = replay_play
cmd-replay-pause-desc = Приостановить повторное воспроизведение.
cmd-replay-pause-help = replay_pause
cmd-replay-toggle-desc = Возобновите или приостановите повторное воспроизведение.
cmd-replay-toggle-help = replay_toggle
cmd-replay-stop-desc = Остановите и загрузите повтор.
cmd-replay-stop-help = replay_stop
cmd-replay-load-desc = Загрузите и запустите повтор.
cmd-replay-load-help = replay_load <replay folder>
cmd-replay-load-hint = Папка воспроизведения.
cmd-replay-skip-desc = Перемещайтесь вперед или назад во времени.
cmd-replay-skip-help = replay_skip <tick or timespan>
cmd-replay-skip-hint = Тики или промежуток времени (HH:MM:SS).
cmd-replay-set-time-desc = Перенеситесь вперед или назад в какое-то определенное время.
cmd-replay-set-time-help = replay_set <tick or time>
cmd-replay-set-time-hint = Тик или промежуток времени (HH:MM:SS), начиная с
cmd-replay-error-time = "{ $time }" это не целое число и не промежуток времени.
cmd-replay-error-args = Неправильное количество аргументов.
cmd-replay-error-no-replay = В данный момент повтор не воспроизводится.
cmd-replay-error-already-loaded = Повтор уже загружен.
cmd-replay-error-run-level = Вы не можете загрузить повтор, пока подключены к серверу.

# Recording commands

cmd-replay-recording-start-desc = Запускает повторную запись, возможно, с некоторым ограничением по времени.
cmd-replay-recording-start-help = Использование: replay_recording_start [name] [overwrite] [time limit]
cmd-replay-recording-start-success = Начал записывать повтор.
cmd-replay-recording-start-already-recording = Уже записываю повтор.
cmd-replay-recording-start-error = При попытке начать запись произошла ошибка.
cmd-replay-recording-start-hint-time = [time limit (minutes)]
cmd-replay-recording-start-hint-name = [name]
cmd-replay-recording-start-hint-overwrite = [overwrite (bool)]
cmd-replay-recording-stop-desc = Останавливает повторную запись.
cmd-replay-recording-stop-help = Использование: replay_recording_stop
cmd-replay-recording-stop-success = Остановлена запись повтора.
cmd-replay-recording-stop-not-recording = В данный момент повтор не записывается.
cmd-replay-recording-stats-desc = Отображает информацию о текущем воспроизведении записи.
cmd-replay-recording-stats-help = Использование: replay_recording_stats
cmd-replay-recording-stats-result = Продолжительность: { $time } мин, Тики: { $ticks }, Размер: { $size } мб, rate: { $rate } мб/мин.
# Time Control UI
replay-time-box-scrubbing-label = Динамическая очистка
replay-time-box-replay-time-label = Время записи: { $current } / { $end }  ({ $percentage }%)
replay-time-box-server-time-label = Серверное время: { $current } / { $end }
replay-time-box-index-label = Индекс: { $current } / { $total }
replay-time-box-tick-label = Галочка: { $current } / { $total }
