mail-recipient-mismatch = Имя получателя или должность не совпадают.
mail-invalid-access = Имя получателя и должность совпадают, но доступ не соответствует требованиям.
mail-locked = Замок от несанкционированного доступа не снимался. Коснитесь ID.
mail-desc-far = Посылка почты. С такого расстояния не разобрать, кому оно адресовано.
mail-desc-close = Посылка почты, адресованная { CAPITALIZE($name) }, { $job }.
mail-desc-fragile = Оно имеет [color=red]красную хрупкую этикетку.[/color].
mail-desc-priority = Замок с защитой от несанкционированного доступа [color=yellow]с желтой приоритетной лентой[/color] активна. Лучше доставить это вовремя!
mail-desc-priority-inactive = Замок с защитой от несанкционированного доступа [color=#886600]с желтой приоритетной лентой[/color] неактивен.
mail-unlocked = Система защиты от взлома разблокирована.
mail-unlocked-by-emag = Система защиты от взлома *БЗЗЗТ*.
mail-unlocked-reward = Система защиты от взлома { $bounty } спесо добавлены на аккаунт отдела снабжения.
mail-penalty-lock = ЗАМОК ЗАЩИТЫ ОТ ВЗЛОМА СЛОМАН. ОТДЕЛ СНАБЖЕНИЯ ПОЛУЧИЛ САНКЦИЮ В { $credits } КРЕДИТОВ.
mail-penalty-fragile = ЦЕЛОСТНОСТЬ НАРУШЕНА. ОТДЕЛ СНАБЖЕНИЯ ПОЛУЧИЛ САНКЦИЮ В { $credits } КРЕДИТОВ.
mail-penalty-expired = СРОК ДОСТАВКИ НАРУШЕН. ОТДЕЛ СНАБЖЕНИЯ ПОЛУЧИЛ САНКЦИЮ В { $credits } КРЕДИТОВ.
mail-item-name-unaddressed = почта
mail-item-name-addressed = почта ({ $recipient })
command-mailto-description = Queue a parcel to be delivered to an entity. Example usage: `mailto 1234 5678 false false`. The target container's contents will be transferred to an actual mail parcel.
command-mailto-help = Usage: { $command } <recipient entityUid> <container entityUid> [is-fragile: true or false] [is-priority: true or false]
command-mailto-no-mailreceiver = Target recipient entity does not have a { $requiredComponent }.
command-mailto-no-blankmail = The { $blankMail } prototype doesn't exist. Something is very wrong. Contact a programmer.
command-mailto-bogus-mail = { $blankMail } did not have { $requiredMailComponent }. Something is very wrong. Contact a programmer.
command-mailto-invalid-container = Target container entity does not have a { $requiredContainer } container.
command-mailto-unable-to-receive = Target recipient entity was unable to be setup for receiving mail. ID may be missing.
command-mailto-no-teleporter-found = Target recipient entity was unable to be matched to any station's mail teleporter. Recipient may be off-station.
command-mailto-success = Success! Mail parcel has been queued for next teleport in { $timeToTeleport } seconds.
command-mailnow = Force all mail teleporters to deliver another round of mail as soon as possible. This will not bypass the undelivered mail limit.
command-mailnow-help = Usage: { $command }
command-mailnow-success = Success! All mail teleporters will be delivering another round of mail soon.
