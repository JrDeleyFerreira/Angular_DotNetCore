import { RedeSocial } from '@app/models/RedeSocial';
import { Lote } from "@app/models/Lote";
import { Palestrante } from '@app/models/Palestrante';

export interface Evento {
  id: number;
  local: string;
  dataEvento?: Date;
  tema: string;
  qtdPessoas: number;
  imagemURL: string;
  telefone: string;
  email: string;
  lotes: Lote[];
  redesSociais: RedeSocial[];
  palestrantesEventos: Palestrante[];
}
