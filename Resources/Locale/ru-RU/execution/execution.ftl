execution-verb-name = Казнить
execution-verb-message = Используйте свое оружие, чтобы казнить кого-либо.

# All the below localisation strings have access to the following variables
# attacker (the person committing the execution)
# victim (the person being executed)
# weapon (the weapon used for the execution)

execution-popup-gun-initial-internal = Вы направляете дуло { THE($weapon) } на голову { $victim }.
execution-popup-gun-initial-external = { $attacker } подносит дуло { THE($weapon) } к глове { $victim }.
execution-popup-gun-complete-internal = Вы стреляете в голову { $victim }!
execution-popup-gun-complete-external = { $attacker } сьреляет в голову { $victim }!
execution-popup-gun-clumsy-internal = Вы промахиваетесь мимо головы { $victim }, и стреляете себе в ногу!
execution-popup-gun-clumsy-external = { $attacker } промахивается по { $victim } и стреляет в ногу { POSS-ADJ($attacker) }!
execution-popup-gun-empty = { CAPITALIZE(THE($weapon)) } щёлкает.
suicide-popup-gun-initial-internal = Вы засовываете { THE($weapon) } себе в рот.
suicide-popup-gun-initial-external = { $attacker } засовывает { THE($weapon) } в рот { POSS-ADJ($attacker) }.
suicide-popup-gun-complete-internal = Вы стреляете себе в голову!
suicide-popup-gun-complete-external = { $attacker } стреляет в голову { REFLEXIVE($attacker) }!
execution-popup-melee-initial-internal = Вы подносите { THE($weapon) } к горлу { $victim }.
execution-popup-melee-initial-external = { $attacker } подносит { POSS-ADJ($attacker) } { $weapon } к горлу { $victim }.
execution-popup-melee-complete-internal = Вы перерезаете горло { $victim }!
execution-popup-melee-complete-external = { $attacker } перерезает горло { $victim }!
suicide-popup-melee-initial-internal = Вы подносите { THE($weapon) } к своему горлу.
suicide-popup-melee-initial-external = { $attacker } подносит { POSS-ADJ($attacker) } { $weapon } к горлу { POSS-ADJ($attacker) }.
suicide-popup-melee-complete-internal = Вы перерезаете себе горло { THE($weapon) }!
suicide-popup-melee-complete-external = { $attacker } перерезает горло { POSS-ADJ($attacker) } с помощью { THE($weapon) }!
