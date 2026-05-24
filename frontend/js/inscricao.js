function ValidacaoCpf(cpf) {
   const cpf = document.getElementById(cpf).value;
    cpf = cpf.replace (/\D/g, ''); // Remove caracteres não numéricos

    if (cpf.length !== 11) {
        console.error('cpf inválido!');
        return
}
let soma = 0;
    for (let i = 0; i < 9; i++) {
        soma += parseInt(cpf.charAt(i)) * (10 - i);
    }
    let resto = 11 - (soma % 11);
    let digito1 = resto === 10 || resto === 11 ? 0 : resto;

    if (digito1 !== parseInt(cpf.charAt(9))) return false;

soma = 0;
    for (let i = 0; i < 10; i++) {
        soma += parseInt(cpf.charAt(i)) * (11 - i);
    }
    resto = 11 - (soma % 11);
    let digito2 = resto === 10 || resto === 11 ? 0 : resto;

    if (digito2 !== parseInt(cpf.charAt(10))) return false;

    return true;
}


function validacaoemail(email) {
    const email = document.getElementById(email).value;
    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return regex.test(email);

const testeEmail = document.getElementById("email").value;
if (validacaoemail(testeEmail)) {}
}

function validacaocep(cep) {

}


function validacaotel (telefone) {

}
 
let cpf = null;
document.addEventListener("DOMContentLoaded", () => {
    carregarAlunos()
    document.getElementById("cadastroForm")
        .addEventListener("submit", savealuno)
})

function savealuno(event){
    event.preventDefault()
    let aluno = JSON.parse(localStorage.getItem("alunos")) || []
    const valor = {
        cpf: document.getElementById("cpf").value,
        nome: document.getElementById("nome").value,
        telefone: document.getElementById("telefone").value,
        email : document.getElementById("email").value,
        cep: document.getElementById("cep").value,
        cidade: document.getElementById("cidade").value,
        bairro: document.getElementById("bairro").value,
        rua: document.getElementById("rua").value,
        numeroresidencia: document.getElementById("numero").value

    }
    if(cpf){
        alunos = alunos.map(c =>
            c.cpf === cpf ? valor : alunos
        )
        cpf = null
        document.querySelector(button[type='submit']).innerText = "Inscrever-se"
        document.getElementById("cpf").disable = false
    } else{
        if(alunos.some(c => c.cpf === valor.cpf)){
            alert("CPF já cadastrado")
            return
        }
        alunos.push(valor)
    }
        localStorage.setItem("alunos", JSON.stringify(alunos))
        document.getElementById("cadastroForm").reset()
        atualizarTabela()
}

