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
character-timer-department-insufficient = Вам нужно на [color=yellow]{ TOSTRING($time, "0") }[/color] больше минут игры на [color={ $departmentColor }]{ $department }[/color] департаменте
character-timer-department-too-high = Вам нужно на [color=yellow]{ TOSTRING($time, "0") }[/color] меньше минут игры на [color={ $departmentColor }]{ $department }[/color] департаменте
character-timer-overall-insufficient = Вам нужно на [color=yellow]{ TOSTRING($time, "0") }[/color] больше общих минут игры
character-timer-overall-too-high = Вам нужно на [color=yellow]{ TOSTRING($time, "0") }[/color] меньше общих минут игры
character-timer-role-insufficient = Вам нужно на [color=yellow]{ TOSTRING($time, "0") }[/color] больше минут игры на [color={ $departmentColor }]{ $job }[/color]
character-timer-role-too-high = Вам нужно на [color=yellow] { TOSTRING($time, "0") }[/color] меньше минут игры на [color={ $departmentColor }]{ $job }[/color]
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
character-logic-and-requirement-listprefix =
    { "" }
    { $indent }[color=gray] и [/color]{ " " }
character-logic-and-requirement =
    Вы должны{ $inverted ->
        [true] { " " }не
       *[other] { "" }
    } подходить [color=red]всем[/color] этим [color=gray]требованиям[/color]: { $options }
character-logic-or-requirement-listprefix =
    { "" }
    { $indent }[color=white]О[/color]{ " " }
character-logic-or-requirement =
    Вы должны{ $inverted ->
        [true] { " " }не
       *[other] { "" }
    } подходить [color=red]хотя бы одному[/color] из [color=white]требований[/color]: { $options }
character-logic-xor-requirement-listprefix =
    { "" }
    { $indent }[color=white]Х[/color]{ " " }
character-logic-xor-requirement =
    Вы должны{ $inverted ->
        [true] { " " }не
       *[other] { "" }
    } подходить [color=red]только одному[/color] из [color=white]требований[/color]: { $options }
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
character-gender-requirement =
    You must { $inverted ->
        [true] not have
       *[other] have
    } the pronouns [color=white]{ $gender }[/color]
character-sex-requirement =
    вы должны{ $inverted ->
        [true] { " " }не
       *[other] { "" }
    } быть [color=white]{ $sex ->
        [None] бесполым
       *[other] { $sex }
    }[/color]
# Whitelist
character-whitelist-requirement =
    Вы { $inverted ->
        [true] не должны
       *[other] должны
    } быть в вайтлисте
character-height-requirement =
    Вы должны{ $inverted ->
        [true] { " " }не
       *[other] { "" }
    } быть { $min ->
        [-2147483648]
            { $max ->
                [2147483648] { "" }
               *[other] ниже [color={ $color }]{ $max }[/color]см
            }
       *[other]
            { $max ->
                [2147483648] выше [color={ $color }]{ $min }[/color]см
               *[other] между [color={ $color }]{ $min }[/color] и [color={ $color }]{ $max }[/color]см в высоту
            }
    }
character-width-requirement =
    Вы должны{ $inverted ->
        [true] { " " }не
       *[other] { "" }
    } быть { $min ->
        [-2147483648]
            { $max ->
                [2147483648] { "" }
               *[other] уже [color={ $color }]{ $max }[/color]см
            }
       *[other]
            { $max ->
                [2147483648] шире [color={ $color }]{ $min }[/color]см
               *[other] между [color={ $color }]{ $min }[/color] и [color={ $color }]{ $max }[/color]см в ширину
            }
    }
character-weight-requirement =
    Вы должны{ $inverted ->
        [true] { " " }не
       *[other] { "" }
    } быть { $min ->
        [-2147483648]
            { $max ->
                [2147483648] { "" }
               *[other] легче [color={ $color }]{ $max }[/color]кг
            }
       *[other]
            { $max ->
                [2147483648] тяжелее [color={ $color }]{ $min }[/color]кг
               *[other] между [color={ $color }]{ $min }[/color] и [color={ $color }]{ $max }[/color]кг
            }
    }
character-item-group-requirement =
    Вы должны { $inverted ->
        [true] иметь { $max } или больше
       *[other] иметь { $max } или меньше
    } предметов из группы [color=white]{ $group }[/color]
