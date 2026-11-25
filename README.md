# RECRIAR

Projeto de cadastro de crianças feito em C#, Windows Forms e MySQL.

## Para rodar

1. Abra `Recriar.sln` no Visual Studio.
2. Defina `Recriar.WinForms` como projeto de inicialização.
3. Se ainda não tiver o banco, execute `Banco/Banco.sql` no MySQL Workbench. Se o banco já estiver instalado, não precisa executar de novo.
4. Crie o arquivo `conexao.txt` dentro de `src/Recriar.WinForms` e coloque os dados do seu MySQL em uma linha:

```text
Server=localhost;Port=3306;Database=recriar_prod;User ID=SEU_USUARIO;Password=SUA_SENHA;
```

Nas propriedades desse arquivo, deixe **Copiar para Diretório de Saída** como **Copiar se for mais recente**. Depois compile e aperte F5.

Cada integrante usa seu próprio usuário e senha. O `conexao.txt` fica só na sua máquina e não deve ser enviado ao GitHub.

## Arquivos

- `Banco`: script completo, consultas e os diagramas MER, DER e EER.
- `src`: código do programa. As telas ficam em `Recriar.WinForms` e a integração com MySQL em `Recriar.Infrastructure`.
- `tests`: testes de integração.

O banco tem 11 tabelas. As telas permitem cadastrar, pesquisar, editar, excluir e exportar a ficha da criança, com responsável, contato de emergência e familiares. As tabelas de oficinas têm dados de exemplo, mas ainda não têm telas.

Em `Banco/Consultas.sql`, as consultas seguem esta ordem: listagem, pesquisa, leitura da ficha (criança, responsável, contato e familiares), parentescos, consultas auxiliares da edição e recuperação do ID gerado.

As validações ficam no C# e os vínculos são conferidos pelas chaves do banco. Como essa versão não usa transações, uma falha durante o salvamento pode deixar parte do cadastro gravada.
