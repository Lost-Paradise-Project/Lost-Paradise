# Job
character-job-requirement =
    Вы должны { $inverted ->
        [true] не быть
       *[other] быть
    } одной из этих работ: { $jobs }
character-department-requirement =
    Вы должны { $inverted ->
        [true] не быть
       *[other] быть
    } в одном из этих департаментов: { $departments }
character-timer-department-insufficient = Вам нужно [color=yellow]{ TOSTRING($time, "0") }[/color] больше минут игры на [color={ $departmentColor }]{ $department }[/color] департаменте
character-timer-department-too-high = Вам нужно [color=yellow]{ TOSTRING($time, "0") }[/color] меньше минут игры на [color={ $departmentColor }]{ $department }[/color] департаменте
character-timer-overall-insufficient = Вам нужно [color=yellow]{ TOSTRING($time, "0") }[/color] больше общих минут игры
character-timer-overall-too-high = Вам нужно [color=yellow]{ TOSTRING($time, "0") }[/color] меньше общих минут игры
character-timer-role-insufficient = Вам нужно [color=yellow]{ TOSTRING($time, "0") }[/color] больше минут игры на [color={ $departmentColor }]{ $job }[/color]
character-timer-role-too-high = Вам нужно [color=yellow] { TOSTRING($time, "0") }[/color] меньше минут игры на [color={ $departmentColor }]{ $job }[/color]
# Profile
character-age-requirement =
    Вы должны { $inverted ->
        [true] не быть
       *[other] быть
    } между [color=yellow]{ $min }[/color] и [color=yellow]{ $max }[/color] годами возраста
character-species-requirement =
    Вы должны { $inverted ->
        [true] не быть
       *[other] быть
    } { $species }
character-trait-requirement =
    Вы должны { $inverted ->
        [true] не использовать
       *[other] использовать
    } один из этих трейтов: { $traits }
character-loadout-requirement =
    Вы должны { $inverted ->
        [true] не использовать
       *[other] использовать
    } один из этих снаряжений: { $loadouts }
character-backpack-type-requirement =
    Вы должны { $inverted ->
        [true] не использовать
       *[other] использовать
    } [color=brown]{ $type }[/color] как вашу сумку
character-clothing-preference-requirement =
    Вы должны { $inverted ->
        [true] не одеть
       *[other] одеть
    } [color=white]{ $type }[/color]
# Whitelist
character-whitelist-requirement =
    Вы { $inverted ->
        [true] не должны
       *[other] должны
    } быть в вайтлисте
