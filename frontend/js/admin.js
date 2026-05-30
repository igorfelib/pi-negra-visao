const KEY = 'nv_eventos';

// Pegar elementos do HTML
const form = document.getElementById('form-evento');
const idInput = document.getElementById('id-evento');
const nomeInput = document.getElementById('nome-evento');
const descInput = document.getElementById('desc-evento');
const dataInput = document.getElementById('data-evento');
const recorrenteInput = document.getElementById('recorrente-evento');
const tabela = document.getElementById('tabela-eventos');

// 1. LISTAR EVENTOS NA TELA
function listar() {
    const eventos = JSON.parse(localStorage.getItem(KEY)) || [];
    tabela.innerHTML = ''; // Limpa a tabela antes de listar

    eventos.forEach((ev, index) => {
        tabela.innerHTML += `
            <tr style="border-bottom: 1px solid #7a5c3e;">
                <td style="padding: 0.5rem;"><strong>${ev.nome}</strong><br><small>${ev.desc}</small></td>
                <td style="padding: 0.5rem;">${ev.data}</td>
                <td style="padding: 0.5rem;">${ev.recorrente}</td>
                <td style="padding: 0.5rem;">
                    <button onclick="editar(${index})" style="background: var(--amarelo); cursor:pointer; padding: 2px 5px;">Editar</button>
                    <button onclick="excluir(${index})" style="background: var(--vermelho); color:white; cursor:pointer; padding: 2px 5px;">X</button>
                </td>
            </tr>
        `;
    });
}

// 2. SALVAR OU ATUALIZAR (ADICIONAR/EDITAR)
form.addEventListener('submit', (e) => {
    e.preventDefault();
    const eventos = JSON.parse(localStorage.getItem(KEY)) || [];
    
    const dados = {
        nome: nomeInput.value,
        desc: descInput.value,
        data: dataInput.value,
        recorrente: recorrenteInput.value
    };

    const indexEdicao = idInput.value;

    if (indexEdicao === "") {
        // Se o campo oculto estiver vazio, adiciona um novo (INSERT)
        eventos.push(dados);
    } else {
        // Se tiver um número lá, atualiza aquela posição (UPDATE)
        eventos[indexEdicao] = dados;
    }

    localStorage.setItem(KEY, JSON.stringify(eventos));
    form.reset();
    idInput.value = ""; // Limpa o ID de edição
    listar();
});

// 3. MANDAR DADOS DA LINHA DE VOLTA PRO FORMULÁRIO PARA EDITAR
function editar(index) {
    const eventos = JSON.parse(localStorage.getItem(KEY)) || [];
    const ev = eventos[index];

    idInput.value = index; // Guarda a posição que estamos editando
    nomeInput.value = ev.nome;
    descInput.value = ev.desc;
    dataInput.value = ev.data;
    recorrenteInput.value = ev.recorrente;
}

// 4. EXCLUIR EVENTO
function excluir(index) {
    const eventos = JSON.parse(localStorage.getItem(KEY)) || [];
    eventos.splice(index, 1); // Remove o item do array
    localStorage.setItem(KEY, JSON.stringify(eventos));
    listar();
}

// Executa a listagem assim que abre a página
listar();