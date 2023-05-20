export interface AorticSurgery {
  Id: number;
  procedure_id: number;
  aneurysm: boolean;
  aneurysm_type: string;
  dissection: boolean;
  dissection_onset: string;
  dissection_type: string;
  coarctation: boolean;
  other_congenital: boolean;
  pathology: string;
  indication: string;
  operative_technique: string;
  range: string;
  stent_graft_technique: string;
}
