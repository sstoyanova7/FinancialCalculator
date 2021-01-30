
export function validate(event) {
    const errors = [];
    
    for (let i = 0; i < event.target.length - 1; i++) {
        if (!/^[+-]?\d*(?:[.,]\d*)?$/.test(event.target[i].value)) {
            errors.push(`${event.target[i].name} must be only numbers!`)
            event.target[i].style.border = 'solid 3px red'
        }
        if (!event.target[i].value) {
            errors.push(`${event.target[i].name} mustn't be empty`)
            event.target[i].style.border = 'solid 3px red'
        }
        if (event.target[i].value <= 0) {
            errors.push(`${event.target[i].name} must be greater than zero`)
            event.target[i].style.border = 'solid 3px red'
        }
    }

    return errors;
}