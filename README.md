**************************************************************************************************************
**************************************************************************************************************
						 _____  ______          _____   __  __ ______ 
						 |  __ \|  ____|   /\   |  __ \ |  \/  |  ____|
						 | |__) | |__     /  \  | |  | || \  / | |__   
						 |  _  /|  __|   / /\ \ | |  | || |\/| |  __|  
						 | | \ \| |____ / ____ \| |__| || |  | | |____ 
						 |_|  \_\______/_/    \_\_____(_)_|  |_|______|
					                                               
**************************************************************************************************************
**************************************************************************************************************
Developer	: João Almeida 

Target 		: Farfetch

Exercice	: Delivery Service 

**************************************************************************************************************
**************************************************************************************************************
DISCLAIMER

	Alguns pontos deste serviço foram simplificados para não complicar desnecessariamente, entre eles:
		Password não encriptadas
			> Para ser mais facil visualizar os dados e alternar de utilizadores.
			> Em LIVE teria de existir essa segurança.
		Connection String HardCoded
			> A Conexão a Base de dados está HardCoded na class "SqlConnectionHelper", 
			pasta "Utils" projecto "DataAccessObject".
			> Em LIVE teria de estar num ficheiro de setting mais acessivel.
		Chamada a API para devolver todos os utilizadores devolve toda a informação sem qualquer 
		verificação e para qualquer pedido sem autorização.
			> Não existe nenhuma policy de accesso a este GET para facilitar o testes e para efeitos de 
			developer.
			> Em LIVE isto não poderia acontecer.
		Token de Autorização expira numa hora
			> Em LIVE teria de se renovar o Token se tivesse quase a expirar mas continuasse em uso.
			> Em LIVE deveria ser alterado consoate as necessidades do serviço.
			> Em LIVE não deveria estar hardcoded mas sim num ficheiro settings mais acessivel.

	Alguns dados já foram inseridos na Aplicação:
		Utilizador Admin 	=>	UserName:Admin | Password:Admin
		Utilizador Default 	=> 	UserName:User  | Password:User
		Nodes A , B, C, D, E, F, G e Z
		Varias conexões entre estes Nodes
		Estrutura visivel no ficheiro "RoutesDemo.jpg"
		**Numeros ao lado das letras corresponde ao ID do Node**


**************************************************************************************************************
**************************************************************************************************************
GETTING STARTED

	Para correr o projecto é necessario:
		> Abrir a solução "FarfetchRoutesService" num ambiente de desenvolvimento, recomenda-se Microsoft 
    Visual Stuido 2017
		> Se já não estiver , indicar o Project API como o StartUp Project
		> Executar 

****************************************************************************************************************************************
****************************************************************************************************************************************ESTRUTURA

	A solução está dividada em camadas estruturadas como projectos , separada pela: API ("API"), Lógica ("BusinessLogicLayer"), Acesso a Dados esta ultima está subdividida em mais duas, uma que preparae requisita os dados ("DataAccessLayer"), e uma outra que realiza a comuniação com os dados ("DataAccessObject"). Além destas camadas existe ainda projectos complementares, são eles o projecto que mantem os modelos ("Models") a serem usados , projecto de testes da lógica ("BusinessLogicLayerUnitTest")

****************************************************************************************************************************************
****************************************************************************************************************************************
DATABASE
	
	A Base de Dados Relacional em SQL está hospedada nos servidores de AZURE, e será acessivel pelo o 
programa sem qualquer alteração a realizar. Se desejar ver os dados "raw" , terá de ser através de um 
ambiente que permita fazer a conexão à BD em questão , no caso do MSV seria atravez do Server Explorer:
	> SQL Server Object Explorer
	> SQL Server
	> Adicionar SQL Server
	> Azure
		> Visto estar a ser usado um mail pessoal não posso divulgar aqui as passwords e mail, mas posso 
		o fazer presencialmente
	
	A aplicação não necessita ter o IDE ligado a DB para funcionar.


****************************************************************************************************************************************
****************************************************************************************************************************************
CRUD E Modelos
	
	A aplicação utiliza os seguintes Modelos : 
		> Node : Nós do Graph
		> Connection : Conexões directionais entre Nós
		> User : Utilizadores da aplicação
		> Path : Devolve o Caminho obitdo
			** Não está a devolver os nós obitdos com nome**


****************************************************************************************************************************************
****************************************************************************************************************************************
PERMISSÕES
	
	Qualquer pedido a API sem qualquer Autorização têm acesso a : 
		User.GetAll , Node.Get , Node.GetAll, Connection.Get, Connection.GetAll , Login
	Qualquer pedido a API com autorização base têm acesso a todos os anteriores mais :
		Path.BestPath.ByTime , Path.BestPath.ByCost
	Qualquer pedido a API com autorização ADMIN têm acesso a Todas as opções


****************************************************************************************************************************************
****************************************************************************************************************************************
ALGORITMO
	
	O Algortimo baseia-se no Algortimo de DIJKSTRA

	Em pseudo Codigo:

	{
		inicia array com distancia até ao cada nó
		inicia array com conexao imediata que o levou até cada nó
		inicia array que indica que nó já foi visitado

		noActual = noOrig

		marca noActual com maxima distancia 
		marca noActual como visitado

		while(true){

			if noActual == noFim 
				break;

			conexoes[] =  obterConexoesLigados(noActual) //Se noActual == noOrgi ignora a conexoes que o levem ao noFim

			if conexoes[].count == 0
				if não há mais nós por visitar
					return NOT_CONNECTED_NODES
				else
					retroce pela conexao que o levao até onde está para o no anterior
					continue

			foreach conexao em conexoes{
				if ( No de fim de Conexao não foi visitado 
					 && a distancia para o no fim da conexao + distancia ate ao no anterior > distancia ate ao no anterior
					 )
					 	  adiciona a distancia ate nó 
					 	  adiciona conexao que o levou nó
			}

			noActual = no com menor distancia

		}
	}



****************************************************************************************************************************************
****************************************************************************************************************************************
TESTES UNITARIOS
	
	Foi utilizado DependacyInjection para realziar testes unitarios, e todos os metodos de Lógica estão testado com um conjunto de possiblidade. 
	Exemplo disto é os Testes Unitarios ao Algortimo que têm um teste para TODAS as possibilidades de sucesso e falha 


****************************************************************************************************************************************
****************************************************************************************************************************************
EXEMPLOS
	
	**BODY deverá ser em JSON(application/json)**

	Alguns exemplos de chamadas ao web service:

		_________________________________________________________________________________________________
		GET 	: https://localhost:44344/api/User 
		
		RETORNA a lista de todos os utilizadores 

		_________________________________________________________________________________________________
		POST 	: https://localhost:44344/api/auth/login
		BODY 	:
		{
        	"username": "Admin",
    	    "password": "Admin",
	    }

	    RETORNA o Token e a indica quando vai expirar

		_________________________________________________________________________________________________
		POST 	: https://localhost:44344/api/User
		HEADER 	: 
			KEY 	: Authorization		&&		Content-Type
			Value	: bearer TOKEN      &&		application/json
		BODY 	:  
		{
        	"username": "XXXXXX",
        	"password": "XXXXXX",
    	    "role": 0
	    }
	    [1 Admin, 0 Normal User]

	    Cria um utilizador com os dados indicados
		_________________________________________________________________________________________________
		POST 	: https://localhost:44344/api/node
		HEADER 	: 
			KEY 	: Authorization		&&		Content-Type
			Value	: bearer TOKEN      &&		application/json

		BODY :
		{
			"name": "X"
		}

		Cria um node com o Nome dado
		_________________________________________________________________________________________________
		POST	: https://localhost:44344/api/connection
		HEADER 	:	 
			KEY 	: Authorization		&&		Content-Type
			Value	: bearer TOKEN      &&		application/json
		BODY	:
		 {
	        "startNode": {
	            "id": X,
	        },
	        "endNode": {
	            "id": X,
	        },
	        "time": X,
	        "cost": X
    	}

    	Cria uma nova Connection directional de Start Node para End Node com Custo e Tempo
		_________________________________________________________________________________________________
		POST 	: https://localhost:44344/api/BestPath/byTime?startNode_ID=X&endNode_ID=X
		HEADER 	:	 
			KEY 	: Authorization		&&		Content-Type
			Value	: bearer TOKEN      &&		application/json

		Retorna o objecto Path com o Custo Total, o Tempo Total, os Nodes Usados e As Connections Usadas

****************************************************************************************************************************************
****************************************************************************************************************************************
					  ______      _____  ______ ______ _______ _____ _    _ 
					 |  ____/\   |  __ \|  ____|  ____|__   __/ ____| |  | |
					 | |__ /  \  | |__) | |__  | |__     | | | |    | |__| |
					 |  __/ /\ \ |  _  /|  __| |  __|    | | | |    |  __  |
					 | | / ____ \| | \ \| |    | |____   | | | |____| |  | |
					 |_|/_/    \_\_|  \_\_|    |______|  |_|  \_____|_|  |_|

****************************************************************************************************************************************
****************************************************************************************************************************************
