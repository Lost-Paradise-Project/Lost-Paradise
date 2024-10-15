# Mailto
command-mailto-description = Поставьте посылку в очередь для доставки юридическому лицу. Пример использования: `mailto 1234 5678 false false`. Содержимое целевого контейнера будет перенесено в реальную почтовую посылку.
command-mailto-help = Использование: { $command } <recipient entityUid> <container entityUid> [is-fragile: true or false] [is-priority: true or false] [is-large: true or false, optional]
command-mailto-no-mailreceiver = Целевой объект не имеет получателя { $requiredComponent }.
command-mailto-no-blankmail = Прототип { $blackmail } не существует. Что-то явно не так. Обратитесь к программисту.
command-mailto-bogus-mail = { $blankMail } не было в { $requiredMailComponent }. Что-то явно не так. Обратитесь к программисту.
command-mailto-invalid-container = Целевой объект-контейнер не имеет { $requiredContainer } контейнера.
command-mailto-unable-to-receive = Не удалось настроить целевой объект-получателя для получения почты. Возможно, отсутствует ID.
command-mailto-no-teleporter-found = Объект-получатель не удалось сопоставить с почтовым телепортом какой-либо станции. Возможно, получатель находится за пределами станции.
command-mailto-success = Успех! Посылка почты поставлена в очередь на следующую телепортацию через { $timeToTeleport } секунд.
# Mailnow
command-mailnow = Заставьте все почтовые телепорты доставить еще одну партию почты как можно скорее. Это не позволит обойти ограничение на количество недоставленных писем.
command-mailnow-help = Использование: { $command }
command-mailnow-success = Успех! Все почтовые телепорты скоро доставят еще одну партию почты.
# Mailtestbulk
command-mailtestbulk = Отправляет по одной посылке каждого типа на указанный почтовый телепортатор.  Неявно вызывает партию почты сейчас.
command-mailtestbulk-help = Использование: { $command } <teleporter_id>
command-mailtestbulk-success = Успех! Все почтовые телепорты скоро доставят еще одну партию почты.
