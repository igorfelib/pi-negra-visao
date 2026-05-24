
function ValidacaoCpf(cpf) {
   const cpf = document.getElementById(cpf).value;
    cpf = cpf.replace (/\D/g, ''); // Remove caracteres não numéricos

    if (cpf.length !== 11) {
        console.error('cpf inválido!');
        return
}
 const digitoverificador = (cpfincompeleto) => {
    let soma = 0;
    for (let i = 0; i < cpfincompeleto.length; i++) {
        let digitoAtual = cpfincompeleto.charAt[i];
    
}}
}

function validacaoemail(email) {
    
}

function validacaocep(cep) { //se tiver cep no formulario

}


function validacaotel (telefone) {

}
 
let cpf = null;
document.addEventListener("DOMContentLoaded", () => {
    carregarClientes()
    document.getElementById("cadastroForm")
        .addEventListener("submit", saveAttCliente)
})

function saveAttCliente(event){
    event.preventDefault()
    let clientes = JSON.parse(localStorage.getItem("clientes")) || []
    const valor = {
        cpf: document.getElementById("cpf").value,
        nome: document.getElementById("nome").value,
        telefone: document.getElementById("telefone").value    
    }
    if(cpf){
        clientes = clientes.map(c =>
            c.cpf === cpf ? valor : clientes
        )
        cpf = null
        document.querySelector(button[type='submit']).innerText = "Salvar"
        document.getElementById("cpf").disable = false
    } else{
        if(clientes.some(c => c.cpf === valor.cpf)){
            alert("CPF já cadastrado")
            return
        }
        clientes.push(valor)
    }
        localStorage.setItem("clientes", JSON.stringify(clientes))
        document.getElementById("cadastroForm").reset()
        atualizarTabela()
}
