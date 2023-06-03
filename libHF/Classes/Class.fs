namespace libHF.Classes


type Class(name: string) =
    let mutable homeworks = List<Homework>.Empty

    member this.Homeworks
        with get (): List<Homework> = homeworks
        and private set value = homeworks <- value

    member val Name = name with get

    member this.AddHomework(hw) = this.Homeworks <- hw :: this.Homeworks
