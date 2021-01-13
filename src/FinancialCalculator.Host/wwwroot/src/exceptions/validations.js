export function checkIfNumber(value) {
    return ! /^[+-]?\d*(?:[.,]\d*)?$/.test(value) ? false : true
}

export function canSubmit(event) {
    let flag = false
  
    for(let i = 0; i < event.target.length - 1; i++) {
        if (event.target[i].style.borderColor === 'red') {
            console.log('Inputs must contain only numbers')
            flag = true
            break
        }
    }
    return flag
}

export function isEmpty(event) {
    let flag = false
    for(let i = 0; i < event.target.length - 1; i++) {
        if(!event.target[i].value) {
            console.log('Inputs must not be empty')
            flag = true
            break
        }
    }
    return flag
}



